using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.Constants;
using BookTradeWebApp.Models;

namespace BookTradeWebApp.Services
{
    public interface IS_BookExchange
    {
        Task<ApiResponse<List<M_BookExchange>>> GetAll();
        Task<ApiResponse<List<M_BookExchangePost>>> GetAllBookExchangePost();
        Task<ApiResponse<M_BookExchange>> Create(M_BookExchange request);
        Task<ApiResponse<M_BookExchangePost>> Post(M_BookExchangePost request);
    }
    public class S_BookExchange : IS_BookExchange
    {
        private readonly HttpClient _httpClient;
        private readonly IS_File _s_File;

        public S_BookExchange(IHttpClientFactory factory, IS_File s_File)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _s_File = s_File;
        }

        public async Task<ApiResponse<List<M_BookExchange>>> GetAll()
        {
            var response = await _httpClient.GetAsync("BookExchange/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_BookExchange>>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_BookExchange>>>();
                return errorResponse!;
            }
        }

        public async Task<ApiResponse<List<M_BookExchangePost>>> GetAllBookExchangePost()
        {
            var response = await _httpClient.GetAsync("BookExchangePost/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_BookExchangePost>>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_BookExchangePost>>>();
                return errorResponse!;
            }
        }

        public async Task<ApiResponse<M_BookExchange>> Create(M_BookExchange request)
        {
            var res = new ApiResponse<M_BookExchange>();

            string imageUrl = await _s_File.UploadImageAsync(request.File!);
            if (string.IsNullOrEmpty(imageUrl))
            {
                res.StatusCode = StatusCodes.Status400BadRequest;
                res.Message = [MessageErrorConstant.INVALID_IMAGE_FILE_FORMAT];
                return res;
            }

            var form = new Dictionary<string, string>
            {
                { "Title", request.Title },
                { "Author", request.Author! },
                { "Description", request.Description! },
                { "Condition", request.Condition.ToString() },
                { "CreatedQuantity", request.CreatedQuantity.ToString() },
                { "ImageUrl", imageUrl },
                { "CategoryId", request.CategoryId!.ToString()! }
            };
            var content = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync("BookExchange/Create", content);

            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<ApiResponse<M_BookExchange>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_BookExchange>>();
                return errorResponse!;
            }
        }

        public async Task<ApiResponse<M_BookExchangePost>> Post(M_BookExchangePost request)
        {
            var res = new ApiResponse<M_BookExchangePost>();

            var form = new Dictionary<string, string>
            {
                { "BookExchangeId", request.BookExchangeId.ToString()! },
                { "ExchangeableQuantity", request.ExchangeableQuantity.ToString() },
                { "Content", request.Content }
            };
            var content = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync("BookExchange/Post", content);

            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<ApiResponse<M_BookExchangePost>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_BookExchangePost>>();
                return errorResponse!;
            }
        }
    }
}
