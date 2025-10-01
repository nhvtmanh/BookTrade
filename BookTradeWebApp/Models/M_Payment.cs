using System.ComponentModel.DataAnnotations;

namespace BookTradeWebApp.Models
{
    public class M_Payment_VNPAY
    {
        public List<int> BookIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; } = null!;
    }
}
