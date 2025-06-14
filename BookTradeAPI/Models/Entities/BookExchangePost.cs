using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class BookExchangePost
{
    public int Id { get; set; }

    public int ExchangeableQuantity { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? BookExchangeId { get; set; }

    public virtual BookExchange? BookExchange { get; set; }
}
