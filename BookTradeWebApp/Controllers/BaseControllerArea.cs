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
    }
}
