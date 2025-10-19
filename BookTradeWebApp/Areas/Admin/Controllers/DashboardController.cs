using BookTradeWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : BaseControllerArea<DashboardController>
    {
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            return View();
        }
    }
}
