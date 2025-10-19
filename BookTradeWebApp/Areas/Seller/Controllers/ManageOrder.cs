using BookTradeWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class ManageOrder : BaseControllerArea<ManageOrder>
    {
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "ManageOrder";
            return View();
        }
    }
}
