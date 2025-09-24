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
    public class OrderController : ControllerBase
    {
        private readonly IS_Order _s_Order;

        public OrderController(IS_Order s_Order)
        {
            _s_Order = s_Order;
        }

        [HttpPost]
        public async Task<IActionResult> CheckStockQuantity(MReq_CheckStockQuantity request)
        {
            request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var res = await _s_Order.CheckStockQuantity(request);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(MReq_PlaceOrder request)
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

            var res = await _s_Order.PlaceOrder(request);
            return Ok(res);
        }
    }
}
