﻿using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class BookExchange
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Author { get; set; }

    public string? Description { get; set; }

    public byte Condition { get; set; }

    public int Quantity { get; set; }

    public int CreatedQuantity { get; set; }

    public int ExchangeableQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CategoryId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<BookExchangeDetail> BookExchangeDetails { get; set; } = new List<BookExchangeDetail>();

    public virtual ICollection<BookExchangePost> BookExchangePosts { get; set; } = new List<BookExchangePost>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderBookExchange> OrderBookExchangeChooseBookExchanges { get; set; } = new List<OrderBookExchange>();

    public virtual ICollection<OrderBookExchange> OrderBookExchangePostBookExchanges { get; set; } = new List<OrderBookExchange>();

    public virtual User? User { get; set; }
}
