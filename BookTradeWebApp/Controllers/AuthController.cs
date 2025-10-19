using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.ModelValidations;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Controllers
{
    public class AuthController : BaseControllerArea<AuthController>
    {
        private readonly IS_Auth _s_Auth;

        public AuthController(IS_Auth s_Auth)
        {
            _s_Auth = s_Auth;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] M_User_Login request)
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

        public IActionResult RegisterSeller()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSeller([FromForm] M_Seller_Register request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<M_Shop>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }

            if (request.File == null || request.File.Length == 0)
            {
                return Ok(new ApiResponse<M_Shop>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = [MessageErrorConstant.NO_FILE_UPLOAD]
                });
            }

            var res = await _s_Auth.RegisterSeller(request);
            return Ok(res);
        }
    }
}
