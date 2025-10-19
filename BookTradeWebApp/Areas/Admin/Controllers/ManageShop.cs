using BookTradeWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageShop : BaseControllerArea<ManageShop>
    {
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "ManageShop";
            return View();
        }
    }
}
