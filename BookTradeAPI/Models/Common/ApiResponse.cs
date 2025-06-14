namespace BookTradeAPI.Models.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public List<string> Message { get; set; } = new List<string>();
    }
}
