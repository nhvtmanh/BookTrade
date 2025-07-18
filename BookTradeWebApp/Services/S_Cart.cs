using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeWebApp.Models;

namespace BookTradeWebApp.Services
{
    public interface IS_Cart
    {
        Task<ApiResponse<int>> AddToCart(M_AddToCart request);
        Task<ApiResponse<Cart>> GetCart();
    }
    public class S_Cart : IS_Cart
    {
        private readonly HttpClient _httpClient;

        public S_Cart(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<int>> AddToCart(M_AddToCart request)
        {
            var res = new ApiResponse<int>();
            var response = await _httpClient.PostAsJsonAsync("Cart/AddToCart", request);

            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
                return errorResponse!;
            }
        }

        public async Task<ApiResponse<Cart>> GetCart()
        {
            var response = await _httpClient.GetAsync("Cart/GetCart");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<Cart>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<Cart>>();
                return errorResponse!;
            }
        }
    }
}
