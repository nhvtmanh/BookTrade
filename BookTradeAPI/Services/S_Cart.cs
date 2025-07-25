using AutoMapper;
using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Utilities.Constants;
using Microsoft.EntityFrameworkCore;

namespace BookTradeAPI.Services
{
    public interface IS_Cart
    {
        Task<ApiResponse<MRes_Cart>> GetCart(int userId);
        Task<ApiResponse<int>> AddToCart(MReq_AddToCart request);
        Task<ApiResponse<int>> UpdateQuantity(MReq_UpdateCartItemQuantity request);
        Task<ApiResponse<int>> DeleteCartItems(MReq_DeleteCartItems request);
    }
    public class S_Cart : IS_Cart
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public S_Cart(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MRes_Cart>> GetCart(int userId)
        {
            var response = new ApiResponse<MRes_Cart>();
            response.StatusCode = StatusCodes.Status200OK;

            var cart = await _dbContext.Carts
                .AsNoTracking()
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
            {
                // Create a new cart
                var newCart = new Cart
                {
                    UserId = userId
                };

                _dbContext.Carts.Add(newCart);
                await _dbContext.SaveChangesAsync();

                response.Data = _mapper.Map<MRes_Cart>(newCart);
                return response;
            }

            response.Data = _mapper.Map<MRes_Cart>(cart);
            return response;
        }

        public async Task<ApiResponse<int>> AddToCart(MReq_AddToCart request)
        {
            var response = new ApiResponse<int>();
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = ["Added to cart"];

            // Check quantity exceeds stock quantity
            var book = await _dbContext.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.BookId);
            if (book == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            int quantity = request.Quantity;
            if (quantity >= book.StockQuantity)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = [MessageErrorConstant.QUANTITY_EXCEED_STOCK];
                return response;
            }

            // Check customer has a cart
            int userId = request.UserId;

            var cart = await _dbContext.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
            {
                // Create a new cart
                var newCart = new Cart
                {
                    UserId = userId
                };

                // Add item to new cart
                newCart.CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        BookId = request.BookId,
                        Quantity = request.Quantity
                    }
                };

                _dbContext.Carts.Add(newCart);
                await _dbContext.SaveChangesAsync();

                response.Data = newCart.Id;
                return response;
            }

            // Check item is already in cart
            var item = cart.CartItems.FirstOrDefault(x => x.BookId == request.BookId);
            if (item != null)
            {
                // Update quantity of existing item
                item.Quantity += request.Quantity;

                await _dbContext.SaveChangesAsync();

                response.Data = cart.Id;
                return response;
            }

            // Add item to cart
            var cartItem = new CartItem
            {
                BookId = request.BookId,
                Quantity = request.Quantity
            };

            cart.CartItems.Add(cartItem);
            await _dbContext.SaveChangesAsync();

            response.Data = cart.Id;
            return response;
        }

        public async Task<ApiResponse<int>> UpdateQuantity(MReq_UpdateCartItemQuantity request)
        {
            var response = new ApiResponse<int>();

            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(x => x.BookId == request.BookId);
            if (cartItem == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            cartItem.Quantity = request.Quantity;

            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = cartItem.Quantity;
            response.Message = [MessageErrorConstant.UPDATE_SUCCESS];
            return response;
        }

        public async Task<ApiResponse<int>> DeleteCartItems(MReq_DeleteCartItems request)
        {
            var response = new ApiResponse<int>();

            // Check customer has a cart
            int userId = request.UserId;

            var cart = await _dbContext.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            // Delete items from cart
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

            _dbContext.CartItems.RemoveRange(cartItems);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = cartItems.Count;
            response.Message = [MessageErrorConstant.DELETE_SUCCESS];
            return response;
        }
    }
}
