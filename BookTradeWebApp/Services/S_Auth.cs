using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.Constants;
using BookTradeWebApp.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace BookTradeWebApp.Services
{
    public interface IS_Auth
    {
        Task<ApiResponse<string>> Login(M_User_Login request);
        Task<ApiResponse<M_Shop>> RegisterSeller(M_Seller_Register request);
    }
    public class S_Auth : IS_Auth
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IS_File _s_File;

        public S_Auth(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor, IS_File s_File)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
            _s_File = s_File;
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

                    if (string.IsNullOrEmpty(role))
                    {
                        successResponse.StatusCode = StatusCodes.Status403Forbidden;
                        successResponse.Message = [MessageErrorConstant.NOT_AUTHORIZED];
                        return successResponse;
                    }

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

        public async Task<ApiResponse<M_Shop>> RegisterSeller(M_Seller_Register request)
        {
            var res = new ApiResponse<M_Shop>();

            string bannerUrl = await _s_File.UploadImageAsync(request.File!);
            if (string.IsNullOrEmpty(bannerUrl))
            {
                res.StatusCode = StatusCodes.Status400BadRequest;
                res.Message = [MessageErrorConstant.INVALID_IMAGE_FILE_FORMAT];
                return res;
            }

            var form = new Dictionary<string, string>
            {
                { "Name", request.Name },
                { "Description", request.Description! },
                { "BannerUrl", bannerUrl },
                { "User.FullName", request.FullName },
                { "User.Address", request.Address },
                { "User.Email", request.Email },
                { "User.Password", request.Password },
                { "User.PhoneNumber", request.PhoneNumber }
            };
            var content = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync("Auth/RegisterSeller", content);

            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<ApiResponse<M_Shop>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Shop>>();
                return errorResponse!;
            }
        }
    }
}
