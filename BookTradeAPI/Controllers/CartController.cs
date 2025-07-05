using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Services;
using BookTradeAPI.Utilities.ModelValidations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTradeAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IS_Cart _s_Cart;

        public CartController(IS_Cart s_Cart)
        {
            _s_Cart = s_Cart;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var res = await _s_Cart.GetCart(userId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(MReq_AddToCart request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<int>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var res = await _s_Cart.AddToCart(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(MReq_UpdateCartItemQuantity request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<int>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Cart.UpdateQuantity(request);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCartItems(MReq_DeleteCartItems request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<int>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var res = await _s_Cart.DeleteCartItems(request);
            return Ok(res);
        }
    }
}
