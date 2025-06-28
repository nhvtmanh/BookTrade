using BookTradeAPI.Models.Entities;

namespace BookTradeAPI.Models.Request
{
    public class MReq_Cart
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public virtual User? User { get; set; }
    }
}
