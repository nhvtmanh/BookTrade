using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.ModelValidations;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Controllers
{
    public class AuthController : BaseController<AuthController>
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
    }
}
