using BookTradeAPI.Models.Common;
using BookTradeAPI.Utilities.Constants;
using BookTradeWebApp.Models;

namespace BookTradeWebApp.Services
{
    public interface IS_Book
    {
        Task<ApiResponse<List<M_Book>>> GetAll();
        Task<ApiResponse<M_Book>> Create(M_Book request);
    }
    public class S_Book : IS_Book
    {
        private readonly HttpClient _httpClient;
        private readonly IS_File _s_File;

        public S_Book(IHttpClientFactory factory, IS_File s_File)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _s_File = s_File;
        }

        public async Task<ApiResponse<List<M_Book>>> GetAll()
        {
            var response = await _httpClient.GetAsync("Book/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_Book>>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_Book>>>();
                return errorResponse!;
            }
        }

        public async Task<ApiResponse<M_Book>> Create(M_Book request)
        {
            var res = new ApiResponse<M_Book>();

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
                { "StockQuantity", request.StockQuantity.ToString() },
                { "BasePrice", request.BasePrice.ToString() },
                { "DiscountPrice", request.DiscountPrice.ToString() },
                { "ImageUrl", imageUrl },
                { "CategoryId", request.CategoryId!.ToString()! }
            };
            var content = new FormUrlEncodedContent(form);
            var response = await _httpClient.PostAsync("Book/Create", content);

            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<ApiResponse<M_Book>>();
                return res!;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Book>>();
                return errorResponse!;
            }
        }
    }
}
