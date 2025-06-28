using BookTradeAPI.Models.Entities;

namespace BookTradeAPI.Models.Response
{
    public class MRes_BookExchangeRequest
    {
        public int Id { get; set; }

        public int? PostBookExchangeDetailId { get; set; }

        public int? ChooseBookExchangeDetailId { get; set; }

        public byte ChooseBookExchangeDetailStatusOld { get; set; }

        public byte Status { get; set; }

        public string? CancelReason { get; set; }

        public virtual BookExchangeDetail? ChooseBookExchangeDetail { get; set; }

        public virtual BookExchangeDetail? PostBookExchangeDetail { get; set; }
    }
}
