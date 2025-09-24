using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookTradeAPI.Services
{
    public interface IS_Order
    {
        Task<ApiResponse<bool>> CheckStockQuantity(MReq_CheckStockQuantity request);
        Task<ApiResponse<int>> PlaceOrder(MReq_PlaceOrder request);
    }
    public class S_Order : IS_Order
    {
        private readonly ApplicationDbContext _dbContext;

        public S_Order(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<bool>> CheckStockQuantity(MReq_CheckStockQuantity request)
        {
            var response = new ApiResponse<bool>();
            response.StatusCode = StatusCodes.Status200OK;

            int userId = request.UserId;
            var cart = await _dbContext.Carts
                .AsNoTracking()
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            var bookIds = request.BookIds;
            var cartItems = cart.CartItems
                .Where(x => bookIds.Contains(x.BookId))
                .ToList();
            if (cartItems == null || cartItems.Count == 0)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            foreach (var item in cartItems)
            {
                var book = item.Book;
                if (item.Quantity >= book.StockQuantity)
                {
                    response.Data = false;
                    response.Message = [MessageErrorConstant.QUANTITY_EXCEED_STOCK];
                    return response;
                }
            }

            response.Data = true;
            return response;
        }

        public async Task<ApiResponse<int>> PlaceOrder(MReq_PlaceOrder request)
        {
            var response = new ApiResponse<int>();
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = ["Your order has been successfully placed"];

            int userId = request.UserId;
            var cart = await _dbContext.Carts
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            var bookIds = request.BookIds;
            var cartItems = cart.CartItems
                .Where(x => bookIds.Contains(x.BookId))
                .ToList();
            if (cartItems == null || cartItems.Count == 0)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            var groupedCartItems = cartItems
                .GroupBy(x => x.Book.ShopId)
                .Select(x => new
                {
                    ShopId = x.Key,
                    CartItems = x.ToList()
                })
                .ToList();
            var orders = new List<Order>();
            var orderDate = DateTime.Now;
            var payment = new Payment
            {
                PaymentDate = orderDate,
                Total = cartItems.Sum(x => x.Quantity * x.Book.DiscountPrice),
                PaymentMethod = (byte)EN_Payment.PaymentMethod.COD,
                Status = (byte)EN_Payment.Status.Pending
            };

            foreach (var group in groupedCartItems)
            {
                var order = new Order
                {
                    OrderDate = orderDate,
                    Status = (byte)EN_Order.Status.Pending,
                    Total = group.CartItems.Sum(x => x.Quantity * x.Book.DiscountPrice),
                    Address = request.Address,
                    BuyerId = userId,
                    ShopId = group.ShopId,
                    Payment = payment,
                    OrderItems = group.CartItems.Select(x => new OrderItem
                    {
                        BookId = x.BookId,
                        Quantity = x.Quantity,
                    }).ToList()
                };
                orders.Add(order);
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Orders.AddRange(orders);
                _dbContext.CartItems.RemoveRange(cartItems);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            response.Data = orders.Count;
            return response;
        }
    }
}
