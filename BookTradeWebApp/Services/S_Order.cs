using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeWebApp.Models;

namespace BookTradeWebApp.Services
{
    public interface IS_Order
    {
        Task<ApiResponse<bool>> CheckStockQuantity(M_CheckStockQuantity request);
        Task<ApiResponse<Order>> PlaceOrder();
    }
    public class S_Order : IS_Order
    {
        private readonly HttpClient _httpClient;

        public S_Order(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<bool>> CheckStockQuantity(M_CheckStockQuantity request)
        {
            var response = await _httpClient.PostAsJsonAsync("Order/CheckStockQuantity", request);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                return errorResponse!;
            }
        }

        public async Task<ApiResponse<Order>> PlaceOrder()
        {
            var response = await _httpClient.GetAsync("Order/PlaceOrder");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<Order>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<Order>>();
                return errorResponse!;
            }
        }
    }
}
