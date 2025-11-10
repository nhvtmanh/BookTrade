using AutoMapper;
using BookTradeAPI.Data;
using BookTradeAPI.Libs.Hubs;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Utilities.Constants;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookTradeAPI.Services
{
    public interface IS_Notification
    {
        Task SendNotification(MReq_Notification request);
        Task SendPlaceOrderSuccessfullyNotification(MReq_Notification_PlaceOrder request);
        Task<ApiResponse<MRes_Notification>> GetAll(int userId);
        Task<ApiResponse<MRes_Notification_Detail>> UpdateIsRead(MReq_Notification_UpdateIsRead request);
    }
    public class S_Notification : IS_Notification
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public S_Notification(IHubContext<NotificationHub> hubContext, ApplicationDbContext dbContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task SendNotification(MReq_Notification request)
        {
            var data = _mapper.Map<Notification>(request);
            data.IsRead = false;
            data.CreatedAt = DateTime.Now;

            _dbContext.Notifications.Add(data);
            await _dbContext.SaveChangesAsync();

            // Send notification by SignalR
            string userId = request.UserId.ToString()!;
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", data);
        }

        public async Task SendPlaceOrderSuccessfullyNotification(MReq_Notification_PlaceOrder request)
        {
            var orderIdToImageUrl = request.OrderIdToImageUrl;
            if (orderIdToImageUrl == null || orderIdToImageUrl.Count == 0)
                return;

            var notifications = new List<Notification>();
            foreach (var item in orderIdToImageUrl)
            {
                int orderId = item.Key;
                string imageUrl = item.Value;

                var notification = new Notification
                {
                    Title = "Order Placed Successfully",
                    Description = $"Order {orderId} has been successfully placed.",
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                    RedirectUrl = $"/member/order/{orderId}",
                    ImageUrl = imageUrl,
                    UserId = request.UserId,
                    OrderId = orderId
                };
                notifications.Add(notification);
            }

            _dbContext.Notifications.AddRange(notifications);
            await _dbContext.SaveChangesAsync();

            // Send notifications by SignalR
            await Task.WhenAll(notifications.Select(x =>
                _hubContext.Clients.User(x.UserId.ToString()!).SendAsync("ReceiveNotification", x)
            ));
        }

        public async Task<ApiResponse<MRes_Notification>> GetAll(int userId)
        {
            var response = new ApiResponse<MRes_Notification>();

            var notifications = await _dbContext.Notifications
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            if (notifications == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = new MRes_Notification
            {
                BadgeCount = notifications.Count(x => !x.IsRead),
                Notifications = _mapper.Map<List<MRes_Notification_Detail>>(notifications)
            };
            return response;
        }

        public async Task<ApiResponse<MRes_Notification_Detail>> UpdateIsRead(MReq_Notification_UpdateIsRead request)
        {
            var response = new ApiResponse<MRes_Notification_Detail>();

            var data = await _dbContext.Notifications
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Id == request.NotificationId);
            if (data == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            data.IsRead = !data.IsRead;
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_Notification_Detail>(data);
            response.Message = [MessageErrorConstant.UPDATE_SUCCESS];
            return response;
        }
    }
}
