using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.ModelValidations;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Controllers
{
    public class BookController : BaseController<BookController>
    {
        private readonly IS_BookExchange _s_BookExchange;

        public BookController(IS_BookExchange s_BookExchange)
        {
            _s_BookExchange = s_BookExchange;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _s_BookExchange.GetAll();
            return Ok(res);
        }

        public async Task<IActionResult> P_Add()
        {
            await SetDropDownCategory();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] M_BookExchange request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<M_BookExchange>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }

            if (request.File == null || request.File.Length == 0)
            {
                return Ok(new ApiResponse<M_BookExchange>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = [MessageErrorConstant.NO_FILE_UPLOAD]
                });
            }

            var res = await _s_BookExchange.Create(request);
            return Ok(res);
        }
    }
}
