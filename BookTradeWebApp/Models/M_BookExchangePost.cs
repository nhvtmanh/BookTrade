using BookTradeAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookTradeWebApp.Models
{
    public class M_BookExchangePost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng trao đổi")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng trao đổi phải lớn hơn 0")]
        public int ExchangeableQuantity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Content { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sách trao đổi")]
        public int? BookExchangeId { get; set; }

        public virtual BookExchange? BookExchange { get; set; }
    }
}
