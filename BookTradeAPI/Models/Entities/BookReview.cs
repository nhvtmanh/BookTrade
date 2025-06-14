using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models.Entities;

public partial class BookReview
{
    public int Id { get; set; }

    public byte Rating { get; set; }

    public string Content { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
