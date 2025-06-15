using BookTradeAPI.Models.Entities;

namespace BookTradeAPI.Models.Response
{
    public class MRes_Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<BookExchange> BookExchanges { get; set; } = new List<BookExchange>();

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
