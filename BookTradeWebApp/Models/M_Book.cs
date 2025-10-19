using BookTradeAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookTradeWebApp.Models
{
    public class M_Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(255, ErrorMessage = "Tiêu đề có độ dài tối đa 255 ký tự")]
        public string Title { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Tên tác giả có độ dài tối đa 100 ký tự")]
        public string? Author { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tình trạng sách")]
        public byte Condition { get; set; }

        public byte Status { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng kho")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng kho phải lớn hơn 0")]
        public int StockQuantity { get; set; }

        public int SoldQuantity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá gốc")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        public decimal DiscountPrice { get; set; }

        public decimal? AverageRating { get; set; }

        public short? TotalRating { get; set; }
        
        public IFormFile? File { get; set; }

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
