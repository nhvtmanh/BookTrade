using System.ComponentModel.DataAnnotations;

namespace BookTradeWebApp.Models
{
    public class M_CheckStockQuantity
    {
        public List<int> BookIds { get; set; } = new List<int>();
    }
    public class M_PlaceOrder
    {
        public List<int> BookIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ có độ dài tối đa 255 ký tự")]
        public string Address { get; set; } = null!;
    }
}
