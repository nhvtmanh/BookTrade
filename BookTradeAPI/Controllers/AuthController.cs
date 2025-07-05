using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Services;
using BookTradeAPI.Utilities.ModelValidations;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IS_Auth _s_Auth;

        public AuthController(IS_Auth s_Auth)
        {
            _s_Auth = s_Auth;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] MReq_User_Register request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_User>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Auth.Register(request);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSeller([FromForm] MReq_Seller_Register request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_Shop>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Auth.RegisterSeller(request);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] MReq_User_Login request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Auth.Login(request);
            return Ok(res);
        }
    }
}
