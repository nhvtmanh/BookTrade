namespace BookTradeWebApp.Models
{
    public class M_Notification
    {
        public int BadgeCount { get; set; }

        public List<M_Notification_Detail> Notifications { get; set; } = new List<M_Notification_Detail>();
    }
    public class M_Notification_Detail
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? RedirectUrl { get; set; }

        public string? ImageUrl { get; set; }
    }
    public class M_Notification_UpdateIsRead
    {
        public int UserId { get; set; }

        public int NotificationId { get; set; }
    }
}
