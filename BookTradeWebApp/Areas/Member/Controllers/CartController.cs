using BookTradeWebApp.Controllers;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Member.Controllers
{
    [Area("Member")]
    public class CartController : BaseControllerArea<CartController>
    {
        private readonly IS_Cart _s_Cart;

        public CartController(IS_Cart s_Cart)
        {
            _s_Cart = s_Cart;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var res = await _s_Cart.GetCart();
            return Ok(res);
        }
    }
}
