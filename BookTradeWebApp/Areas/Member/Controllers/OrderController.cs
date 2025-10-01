using BookTradeWebApp.Controllers;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Member.Controllers
{
    [Area("Member")]
    public class OrderController : BaseControllerArea<OrderController>
    {
        private readonly IS_Order _s_Order;

        public OrderController(IS_Order s_Order)
        {
            _s_Order = s_Order;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckStockQuantity(M_CheckStockQuantity request)
        {
            var res = await _s_Order.CheckStockQuantity(request);
            return Ok(res);
        }
    }
}
