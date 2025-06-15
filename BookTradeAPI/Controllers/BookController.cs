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
    public class BookController : ControllerBase
    {
        private readonly IS_Book _s_Book;

        public BookController(IS_Book s_Book)
        {
            _s_Book = s_Book;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _s_Book.GetAll();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _s_Book.GetById(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MReq_Book request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_Book>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Book.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] MReq_Book request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_Book>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Book.Update(request);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_Book.Delete(id);
            return Ok(res);
        }
    }
}
