using BookTradeAPI.Models.Common;
using BookTradeWebApp.Models;

namespace BookTradeWebApp.Services
{
    public interface IS_Payment
    {
        Task<ApiResponse<string>> CreatePaymentUrl(M_Payment_VNPAY request);
    }
    public class S_Payment : IS_Payment
    {
        private readonly HttpClient _httpClient;

        public S_Payment(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<string>> CreatePaymentUrl(M_Payment_VNPAY request)
        {
            var response = await _httpClient.PostAsJsonAsync("Payment/CreatePaymentUrl", request);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
                return errorResponse!;
            }
        }
    }
}
