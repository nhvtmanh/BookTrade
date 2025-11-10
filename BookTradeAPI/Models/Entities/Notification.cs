using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? RedirectUrl { get; set; }

    public string? ImageUrl { get; set; }

    public int? UserId { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }
}
