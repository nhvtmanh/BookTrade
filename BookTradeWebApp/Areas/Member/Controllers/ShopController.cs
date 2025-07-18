using BookTradeWebApp.Controllers;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Member.Controllers
{
    [Area("Member")]
    public class ShopController : BaseControllerArea<ShopController>
    {
        private readonly IS_Book _s_Book;
        private readonly IS_Cart _s_Cart;

        public ShopController(IS_Book s_Book, IS_Cart s_Cart)
        {
            _s_Book = s_Book;
            _s_Cart = s_Cart;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Shop";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _s_Book.GetAll();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(M_AddToCart request)
        {
            var res = await _s_Cart.AddToCart(request);
            return Ok(res);
        }
    }
}
