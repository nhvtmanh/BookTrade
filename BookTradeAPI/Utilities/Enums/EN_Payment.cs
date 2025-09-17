namespace BookTradeAPI.Utilities.Enums
{
    public class EN_Payment
    {
        public enum PaymentMethod
        {
            COD,
            Online
        }
        public enum Status
        {
            Pending,
            Paid,
            Failed
        }
    }
}
