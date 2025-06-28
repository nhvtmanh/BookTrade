using BookTradeAPI.Models.Common;
using BookTradeWebApp.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace BookTradeWebApp.Services
{
    public interface IS_Auth
    {
        Task<ApiResponse<string>> Login(M_User_Login request);
    }
    public class S_Auth : IS_Auth
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public S_Auth(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<string>> Login(M_User_Login request)
        {
            var form = new Dictionary<string, string>
            {
                { "Email", request.Email },
                { "Password", request.Password }
            };
            var content = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync("Auth/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<string>();
                string responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiResponse<string>>(responseData);

                if (data!.StatusCode == StatusCodes.Status200OK)
                {
                    string token = data!.Data!;

                    //Decode token using JsonWebTokenHandler
                    var handler = new JsonWebTokenHandler();
                    var jwt = handler.ReadJsonWebToken(token);
                    var roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
                    var role = jwt.Claims.FirstOrDefault(c => c.Type == roleClaimType)?.Value;

                    //Store token in session
                    _httpContextAccessor.HttpContext?.Session.SetString("token", token);

                    successResponse.Data = role;
                }

                successResponse.StatusCode = data.StatusCode;
                successResponse.Message = data.Message;
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
                return errorResponse!;
            }
        }
    }
}
