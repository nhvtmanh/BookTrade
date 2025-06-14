using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class BookExchangeDetail
{
    public int Id { get; set; }

    public byte Status { get; set; }

    public int? BookExchangeId { get; set; }

    public virtual BookExchange? BookExchange { get; set; }
}
