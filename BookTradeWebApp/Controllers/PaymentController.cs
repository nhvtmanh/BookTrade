using Microsoft.AspNetCore.Mvc;
using VNPAY.NET;

namespace BookTradeWebApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;

        public PaymentController(IVnpay vnpay, IConfiguration configuration)
        {
            _vnpay = vnpay;
            _configuration = configuration;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"]!, _configuration["Vnpay:HashSecret"]!, _configuration["Vnpay:BaseUrl"]!, _configuration["Vnpay:CallbackUrl"]!);
        }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);

                    if (paymentResult.IsSuccess)
                    {
                        return View("SuccessPayment");
                    }

                    return View("FailedPayment");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}
