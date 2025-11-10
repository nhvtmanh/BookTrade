using BookTradeWebApp.Controllers;
using BookTradeWebApp.Models;
using BookTradeWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTradeWebApp.Areas.Member.Controllers
{
    [Area("Member")]
    public class NotificationController : BaseControllerArea<NotificationController>
    {
        private readonly IS_Notification _s_Notification;

        public NotificationController(IS_Notification s_Notification)
        {
            _s_Notification = s_Notification;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _s_Notification.GetAll();
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIsRead(M_Notification_UpdateIsRead request)
        {
            var res = await _s_Notification.UpdateIsRead(request);
            return Ok(res);
        }
    }
}
