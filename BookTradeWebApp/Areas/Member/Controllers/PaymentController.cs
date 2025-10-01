using BookTradeWebApp.Controllers;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VNPAY.NET;
using VNPAY.NET.Models;

namespace BookTradeWebApp.Areas.Member.Controllers
{
    [Area("Member")]
    public class PaymentController : BaseControllerArea<PaymentController>
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly IS_Payment _s_Payment;

        public PaymentController(IVnpay vnpay, IConfiguration configuration, IS_Payment s_Payment)
        {
            _vnpay = vnpay;
            _configuration = configuration;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"]!, _configuration["Vnpay:HashSecret"]!, _configuration["Vnpay:BaseUrl"]!, _configuration["Vnpay:CallbackUrl"]!);

            _s_Payment = s_Payment;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(M_Payment_VNPAY request)
        {
            var res = await _s_Payment.CreatePaymentUrl(request);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                    TempData["PaymentResult"] = JsonConvert.SerializeObject(paymentResult);

                    if (paymentResult.IsSuccess)
                    {
                        return RedirectToAction("SuccessPayment", "Payment");
                    }

                    return RedirectToAction("FailedPayment", "Payment");
                }
                catch
                {
                    return RedirectToAction("FailedPayment", "Payment");
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        public IActionResult SuccessPayment()
        {
            var paymentResultJson = TempData["PaymentResult"] as string;
            if (paymentResultJson == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            var paymentResult = JsonConvert.DeserializeObject<PaymentResult>(paymentResultJson);
            return View(paymentResult);
        }

        public IActionResult FailedPayment()
        {
            var paymentResultJson = TempData["PaymentResult"] as string;
            if (paymentResultJson == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            var paymentResult = JsonConvert.DeserializeObject<PaymentResult>(paymentResultJson);
            return View(paymentResult);
        }
    }
}
