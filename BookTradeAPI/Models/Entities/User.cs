using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class User : IdentityUser<int>
{
    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public virtual ICollection<BookExchange> BookExchanges { get; set; } = new List<BookExchange>();

    public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderBookExchange> OrderBookExchangeChooseUsers { get; set; } = new List<OrderBookExchange>();

    public virtual ICollection<OrderBookExchange> OrderBookExchangePostUsers { get; set; } = new List<OrderBookExchange>();

    public virtual ICollection<Order> OrderBuyers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSellers { get; set; } = new List<Order>();

    public virtual ICollection<Book> BooksNavigation { get; set; } = new List<Book>();
}
