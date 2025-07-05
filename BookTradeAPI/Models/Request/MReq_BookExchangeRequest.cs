using System.ComponentModel.DataAnnotations;

namespace BookTradeAPI.Models.Request
{
    public class MReq_BookExchangeRequest
    {
        public int Id { get; set; }

        public int? PostBookExchangeDetailId { get; set; }

        public int? ChooseBookExchangeDetailId { get; set; }

        [StringLength(100, ErrorMessage = "Lý do hủy có độ dài tối đa 100 ký tự")]
        public string? CancelReason { get; set; }
    }
}
