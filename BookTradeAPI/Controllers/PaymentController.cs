using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Services;
using BookTradeAPI.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace BookTradeAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly IS_Payment _s_Payment;

        public PaymentController(IVnpay vnpay, IConfiguration configuration, IS_Payment s_Payment)
        {
            _vnpay = vnpay;
            _configuration = configuration;
            _s_Payment = s_Payment;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"]!, _configuration["Vnpay:HashSecret"]!, _configuration["Vnpay:BaseUrl"]!, _configuration["Vnpay:CallbackUrl"]!);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<string>> CreatePaymentUrl(MReq_Payment request)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch

                request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                // Get total money of checked cart items
                var getCartTotalResponse = await _s_Payment.GetCartTotal(request);
                if (getCartTotalResponse.StatusCode != StatusCodes.Status200OK)
                {
                    return BadRequest(getCartTotalResponse.Message);
                }
                double money = getCartTotalResponse.Data;

                var data = JsonConvert.DeserializeObject<MReq_Payment_Description>(request.Description);
                data!.UserId = request.UserId;
                string description = JsonConvert.SerializeObject(data);

                var paymentRequest = new PaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = money,
                    Description = description,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
                };

                var paymentUrl = _vnpay.GetPaymentUrl(paymentRequest);

                var res = new ApiResponse<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = paymentUrl,
                    Message = [MessageErrorConstant.CREATE_SUCCESS]
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);

                    if (paymentResult.IsSuccess)
                    {
                        // Thực hiện hành động nếu thanh toán thành công tại đây. Ví dụ: Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu.
                        await _s_Payment.HandleSuccessPayment(paymentResult);
                        return Ok(new
                        {
                            message = "Thanh toán thành công"
                        });
                    }

                    // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.
                    await _s_Payment.HandleFailedPayment(paymentResult);
                    return BadRequest(new
                    {
                        message = "Thanh toán thất bại"
                    });
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
