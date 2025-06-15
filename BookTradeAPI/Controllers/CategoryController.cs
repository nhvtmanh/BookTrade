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
    public class CategoryController : ControllerBase
    {
        private readonly IS_Category _s_Category;

        public CategoryController(IS_Category s_Category)
        {
            _s_Category = s_Category;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _s_Category.GetAll();
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _s_Category.GetById(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MReq_Category request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_Category>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Category.Create(request);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] MReq_Category request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<MRes_Category>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }
            var res = await _s_Category.Update(request);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _s_Category.Delete(id);
            return Ok(res);
        }
    }
}
