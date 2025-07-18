using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookTradeWebApp.Controllers
{
    public abstract class BaseControllerArea<T> : Controller where T : BaseControllerArea<T>
    {
        protected async Task SetDropDownCategory()
        {
            var res = await HttpContext.RequestServices.GetService<IS_Category>()!.GetAll();
            if (res.StatusCode == StatusCodes.Status200OK)
            {
                var categories = res.Data;
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
            }
        }

        protected async Task SetDropDownBookExchange()
        {
            var res = await HttpContext.RequestServices.GetService<IS_BookExchange>()!.GetAll();
            if (res.StatusCode == StatusCodes.Status200OK)
            {
                var bookExchanges = res.Data;
                ViewBag.BookExchanges = new SelectList(bookExchanges, "Id", "Title");
            }
        }
    }
}
