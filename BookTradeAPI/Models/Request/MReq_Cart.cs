using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_AddToCart
    {
        public int UserId { get; set; }

        public int BookId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
    public class MReq_UpdateCartItemQuantity
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
    public class MReq_DeleteCartItems
    {
        public int UserId { get; set; }

        public List<int> BookIds { get; set; } = new List<int>();
    }
}
