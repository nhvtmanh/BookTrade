using BookTradeAPI.Models.Common;
using BookTradeWebApp.Models;

namespace BookTradeWebApp.Services
{
    public interface IS_Category
    {
        Task<ApiResponse<List<M_Category>>> GetAll();
    }
    public class S_Category : IS_Category
    {
        private readonly HttpClient _httpClient;

        public S_Category(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<List<M_Category>>> GetAll()
        {
            var response = await _httpClient.GetAsync("Category/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_Category>>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_Category>>>();
                return errorResponse!;
            }
        }
    }
}
