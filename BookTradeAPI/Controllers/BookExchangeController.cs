using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Services;
using BookTradeAPI.Utilities.ModelValidations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookExchangeController : ControllerBase
    {
        private readonly IS_BookExchange _s_BookExchange;

        public BookExchangeController(IS_BookExchange s_BookExchange)
        {
            _s_BookExchange = s_BookExchange;
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
