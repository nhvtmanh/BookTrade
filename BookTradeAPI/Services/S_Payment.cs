using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VNPAY.NET.Models;

namespace BookTradeAPI.Services
{
    public interface IS_Payment
    {
        Task<ApiResponse<double>> GetCartTotal(MReq_Payment request);
        Task HandleSuccessPayment(PaymentResult paymentResult);
        Task HandleFailedPayment(PaymentResult paymentResult);
    }
    public class S_Payment : IS_Payment
    {
        private readonly ApplicationDbContext _dbContext;

        public S_Payment(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<double>> GetCartTotal(MReq_Payment request)
        {
            var response = new ApiResponse<double>();
            response.StatusCode = StatusCodes.Status400BadRequest;
            response.Message = [MessageErrorConstant.NOT_FOUND];

            int userId = request.UserId;

            var cart = await _dbContext.Carts
                .AsNoTracking()
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
                return response;

            var bookIds = request.BookIds;
            var checkedCartItems = cart.CartItems
                .Where(x => bookIds.Contains(x.BookId))
                .ToList();
            if (checkedCartItems == null || checkedCartItems.Count == 0)
                return response;

            double total = (double)checkedCartItems.Sum(x => x.Book.DiscountPrice * x.Quantity);
            response.StatusCode = StatusCodes.Status200OK;
            response.Data = total;
            return response;
        }

        public async Task HandleSuccessPayment(PaymentResult paymentResult)
        {
            var description = JsonConvert.DeserializeObject<MReq_Payment_Description>(paymentResult.Description);
            var bookIds = description!.BookIds;
            var shippingAddress = description.ShippingAddress;
            if (bookIds == null || bookIds.Count == 0 || string.IsNullOrEmpty(shippingAddress))
                return;

            var cartItems = await _dbContext.CartItems
                .Where(x => bookIds.Contains(x.BookId))
                .Include(x => x.Book)
                .Include(x => x.Cart)
                .ToListAsync();
            if (cartItems == null || cartItems.Count == 0)
                return;

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
                PaymentDate = paymentResult.Timestamp,
                Total = cartItems.Sum(x => x.Book.DiscountPrice * x.Quantity),
                PaymentMethod = (byte)EN_Payment.PaymentMethod.Online,
                Status = (byte)EN_Payment.Status.Paid
            };

            foreach (var group in groupedCartItems)
            {
                var order = new Order
                {
                    OrderDate = orderDate,
                    Status = (byte)EN_Order.Status.Pending,
                    Total = group.CartItems.Sum(x => x.Book.DiscountPrice * x.Quantity),
                    Address = shippingAddress,
                    BuyerId = group.CartItems[0].Cart.UserId,
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
        }

        public async Task HandleFailedPayment(PaymentResult paymentResult)
        {
            var description = JsonConvert.DeserializeObject<MReq_Payment_Description>(paymentResult.Description);
            var bookIds = description!.BookIds;
            if (bookIds == null || bookIds.Count == 0)
                return;

            var cartItems = await _dbContext.CartItems
                .AsNoTracking()
                .Where(x => bookIds.Contains(x.BookId))
                .Include(x => x.Book)
                .ToListAsync();
            if (cartItems == null || cartItems.Count == 0)
                return;

            var payment = new Payment
            {
                PaymentDate = paymentResult.Timestamp,
                Total = cartItems.Sum(x => x.Book.DiscountPrice * x.Quantity),
                PaymentMethod = (byte)EN_Payment.PaymentMethod.Online,
                Status = (byte)EN_Payment.Status.Failed
            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
