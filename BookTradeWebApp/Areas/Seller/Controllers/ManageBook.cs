using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.ModelValidations;
using BookTradeWebApp.Controllers;
using BookTradeWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class ManageBook : BaseControllerArea<ManageBook>
    {
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "ManageBook";
            return View();
        }

        //public async Task<IActionResult> P_Add()
        //{
        //    await SetDropDownCategory();
        //    return PartialView("~/Views/Seller/ManageBook/P_Add.cshtml");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromForm] M_Book request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Ok(new ApiResponse<M_Book>
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Message = ModelValidationErrorMessage.GetErrorMessage(ModelState)
        //        });
        //    }

        //    if (request.File == null || request.File.Length == 0)
        //    {
        //        return Ok(new ApiResponse<M_Book>
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Message = [MessageErrorConstant.NO_FILE_UPLOAD]
        //        });
        //    }

        //    var res = await _s_Book.Create(request);
        //    return Ok(res);
        //}
    }
}
