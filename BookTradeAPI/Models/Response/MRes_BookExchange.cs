using BookTradeAPI.Models.Entities;

namespace BookTradeAPI.Models.Response
{
    public class MRes_BookExchange
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Author { get; set; }

        public string? Description { get; set; }

        public byte Condition { get; set; }

        public int Quantity { get; set; }

        public int? CreatedQuantity { get; set; }

        public int? ExchangeableQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CategoryId { get; set; }

        public int? UserId { get; set; }

        public virtual ICollection<BookExchangeDetail> BookExchangeDetails { get; set; } = new List<BookExchangeDetail>();

        public virtual Category? Category { get; set; }

        public virtual User? User { get; set; }
    }
}
