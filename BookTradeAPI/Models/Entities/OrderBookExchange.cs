using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class OrderBookExchange
{
    public int Id { get; set; }

    public DateTime ExchangeDate { get; set; }

    public int? PostUserId { get; set; }

    public int? PostBookExchangeId { get; set; }

    public byte PostOrderStatus { get; set; }

    public int? ChooseUserId { get; set; }

    public int? ChooseBookExchangeId { get; set; }

    public byte ChooseOrderStatus { get; set; }

    public virtual BookExchange? ChooseBookExchange { get; set; }

    public virtual User? ChooseUser { get; set; }

    public virtual BookExchange? PostBookExchange { get; set; }

    public virtual User? PostUser { get; set; }
}
