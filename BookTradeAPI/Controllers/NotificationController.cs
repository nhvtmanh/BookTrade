using BookTradeAPI.Models.Request;
using BookTradeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTradeAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IS_Notification _s_Notification;

        public NotificationController(IS_Notification s_Notification)
        {
            _s_Notification = s_Notification;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var res = await _s_Notification.GetAll(userId);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIsRead(MReq_Notification_UpdateIsRead request)
        {
            request.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var res = await _s_Notification.UpdateIsRead(request);
            return Ok(res);
        }
    }
}
