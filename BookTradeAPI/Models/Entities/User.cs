using Microsoft.AspNetCore.Identity;

namespace BookTradeAPI.Models.Entities;

public partial class User : IdentityUser<int>
{
    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
