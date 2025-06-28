using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.ModelValidations;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Controllers
{
    public class BookExchangeController : BaseController<BookExchangeController>
    {
        private readonly IS_BookExchange _s_BookExchange;

        public BookExchangeController(IS_BookExchange s_BookExchange)
        {
            _s_BookExchange = s_BookExchange;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookExchangePost()
        {
            var res = await _s_BookExchange.GetAllBookExchangePost();
            return Ok(res);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> P_Post()
        {
            await SetDropDownBookExchange();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] M_BookExchangePost request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse<M_BookExchangePost>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
                });
            }

            var res = await _s_BookExchange.Post(request);
            return Ok(res);
        }
    }
}
