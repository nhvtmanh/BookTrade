using BookTradeAPI.Models.Common;
using BookTradeWebApp.Models;

namespace BookTradeWebApp.Services
{
    public interface IS_Notification
    {
        Task<ApiResponse<M_Notification>> GetAll();
        Task<ApiResponse<M_Notification_Detail>> UpdateIsRead(M_Notification_UpdateIsRead request);
    }
    public class S_Notification : IS_Notification
    {
        private readonly HttpClient _httpClient;

        public S_Notification(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<M_Notification>> GetAll()
        {
            var response = await _httpClient.GetAsync("Notification/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<M_Notification>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Notification>>();
                return errorResponse!;
            }
        }

        public async Task<ApiResponse<M_Notification_Detail>> UpdateIsRead(M_Notification_UpdateIsRead request)
        {
            var response = await _httpClient.PutAsJsonAsync("Notification/UpdateIsRead", request);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<M_Notification_Detail>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Notification_Detail>>();
                return errorResponse!;
            }
        }
    }
}
