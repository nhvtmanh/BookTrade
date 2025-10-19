using BookTradeWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class DashboardController : BaseControllerArea<DashboardController>
    {
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            return View();
        }
    }
}
