﻿using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public byte Status { get; set; }

    public decimal Total { get; set; }

    public string Address { get; set; } = null!;

    public int? BuyerId { get; set; }

    public int? ShopId { get; set; }

    public int? PaymentId { get; set; }

    public virtual User? Buyer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual Shop? Shop { get; set; }
}
