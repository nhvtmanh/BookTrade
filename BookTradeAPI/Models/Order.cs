using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public byte Status { get; set; }

    public decimal Total { get; set; }

    public int? BuyerId { get; set; }

    public int? SellerId { get; set; }

    public int? PaymentId { get; set; }

    public virtual User? Buyer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual User? Seller { get; set; }
}
