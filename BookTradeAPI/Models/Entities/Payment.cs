using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal Total { get; set; }

    public byte PaymentMethod { get; set; }

    public byte Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
