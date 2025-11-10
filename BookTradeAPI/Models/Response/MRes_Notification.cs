namespace BookTradeAPI.Models.Response
{
    public class MRes_Notification
    {
        public int BadgeCount { get; set; }

        public List<MRes_Notification_Detail> Notifications { get; set; } = new List<MRes_Notification_Detail>();
    }
    public class MRes_Notification_Detail
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? RedirectUrl { get; set; }

        public string? ImageUrl { get; set; }
    }
}
