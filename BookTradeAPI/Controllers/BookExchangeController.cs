using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
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
    public class BookExchangeController : ControllerBase
    {
        private readonly IS_BookExchange _s_BookExchange;

        public BookExchangeController(IS_BookExchange s_BookExchange)
        {
            _s_BookExchange = s_BookExchange;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _s_BookExchange.GetAll();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MReq_BookExchange request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_BookExchange>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var res = await _s_BookExchange.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] MReq_BookExchange request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_BookExchange>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_BookExchange.Update(request);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MReq_BookExchangePost request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_BookExchangePost>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_BookExchange.Post(request);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_BookExchange.Delete(id);
            return Ok(res);
        }
    }
}
