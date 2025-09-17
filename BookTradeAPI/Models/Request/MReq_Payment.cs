using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_Payment
    {
        public int UserId { get; set; }

        public List<int> BookIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; } = null!;
    }
    public class MReq_Payment_Description
    {
        public List<int> BookIds { get; set; } = new List<int>();
        public string ShippingAddress { get; set; } = null!;
    }
}
