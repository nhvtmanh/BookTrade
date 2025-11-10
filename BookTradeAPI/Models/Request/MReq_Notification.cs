namespace BookTradeAPI.Models.Request
{
    public class MReq_Notification
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? RedirectUrl { get; set; }

        public string? ImageUrl { get; set; }

        public int? UserId { get; set; }

        public int? OrderId { get; set; }
    }
    public class MReq_Notification_PlaceOrder
    {
        public Dictionary<int, string> OrderIdToImageUrl { get; set; } = new Dictionary<int, string>();

        public int? UserId { get; set; }
    }
    public class MReq_Notification_UpdateIsRead
    {
        public int UserId { get; set; }

        public int NotificationId { get; set; }
    }
}
