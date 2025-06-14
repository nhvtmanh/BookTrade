using BookTradeAPI.Models.Entities;

namespace BookTradeAPI.Models.Response
{
    public class MRes_BookExchangePost
    {
        public int Id { get; set; }

        public int ExchangeableQuantity { get; set; }

        public string Content { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public int? BookExchangeId { get; set; }

        public virtual BookExchange? BookExchange { get; set; }
    }
}
