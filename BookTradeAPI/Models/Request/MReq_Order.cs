using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_PlaceOrder
    {
        public int UserId { get; set; }

        public List<int> BookIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ có độ dài tối đa 255 ký tự")]
        public string Address { get; set; } = null!;
    }
    public class MReq_CheckStockQuantity
    {
        public int UserId { get; set; }

        public List<int> BookIds { get; set; } = new List<int>();
    }
}
