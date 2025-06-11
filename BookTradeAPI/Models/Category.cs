using System;
using System.Collections.Generic;

namespace BookTradeAPI.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookExchange> BookExchanges { get; set; } = new List<BookExchange>();

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
