using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class Shop
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? BannerUrl { get; set; }

    public byte Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
