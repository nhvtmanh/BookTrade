using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_BookExchangePost
    {
        [Required(ErrorMessage = "Vui lòng chọn sách trao đổi")]
        public int BookExchangeId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng trao đổi")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng trao đổi phải lớn hơn 0")]
        public int ExchangeableQuantity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Content { get; set; } = null!;
    }
}
