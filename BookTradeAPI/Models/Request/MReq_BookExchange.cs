using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_BookExchange
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(255, ErrorMessage = "Tiêu đề có độ dài tối đa 255 ký tự")]
        public string Title { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Tên tác giả có độ dài tối đa 100 ký tự")]
        public string? Author { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tình trạng sách")]
        public byte Condition { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }

        public int? UserId { get; set; }
    }
}
