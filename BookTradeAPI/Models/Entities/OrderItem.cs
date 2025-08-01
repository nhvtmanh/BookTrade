﻿using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class OrderItem
{
    public int OrderId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
