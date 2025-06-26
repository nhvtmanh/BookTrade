using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Controllers
{
    public class BookExchangeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
