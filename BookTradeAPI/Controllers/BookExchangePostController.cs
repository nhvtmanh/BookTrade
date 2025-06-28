using BookTradeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookExchangePostController : ControllerBase
    {
        private readonly IS_BookExchangePost _s_BookExchangePost;

        public BookExchangePostController(IS_BookExchangePost s_BookExchangePost)
        {
            _s_BookExchangePost = s_BookExchangePost;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _s_BookExchangePost.GetAll();
            return Ok(res);
        }
    }
}
