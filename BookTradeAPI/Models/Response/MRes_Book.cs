using BookTradeAPI.Models.Entities;

namespace BookTradeAPI.Models.Response
{
    public class MRes_Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Author { get; set; }

        public string? Description { get; set; }

        public byte Condition { get; set; }

        public byte Status { get; set; }

        public int StockQuantity { get; set; }

        public int SoldQuantity { get; set; }

        public decimal BasePrice { get; set; }

        public decimal DiscountPrice { get; set; }

        public decimal? AverageRating { get; set; }

        public short? TotalRating { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CategoryId { get; set; }

        public int? ShopId { get; set; }

        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public virtual Category? Category { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public virtual Shop? Shop { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
