using AutoMapper;
using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BookTradeAPI.Services
{
    public interface IS_BookExchange
    {
        Task<ApiResponse<MRes_BookExchange>> Create(MReq_BookExchange request);
        Task<ApiResponse<MRes_BookExchangePost>> Post(MReq_BookExchangePost request);
    }
    public class S_BookExchange : IS_BookExchange
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public S_BookExchange(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MRes_BookExchange>> Create(MReq_BookExchange request)
        {
            var response = new ApiResponse<MRes_BookExchange>();

            var data = _mapper.Map<BookExchange>(request);
            data.CreatedAt = DateTime.Now;

            // Create book exchange details with status "Created"
            data.BookExchangeDetails = Enumerable.Range(1, data.Quantity)
                .Select(x => new BookExchangeDetail
                {
                    Status = (byte)EN_BookExchangeDetail.Status.Created
                })
                .ToList();

            // Set book exchange created quantity
            data.CreatedQuantity = data.Quantity;

            _dbContext.BookExchanges.Add(data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status201Created;
            response.Data = _mapper.Map<MRes_BookExchange>(data);
            response.Message = [MessageErrorConstant.CREATE_SUCCESS];
            return response;
        }

        public async Task<ApiResponse<MRes_BookExchangePost>> Post(MReq_BookExchangePost request)
        {
            var response = new ApiResponse<MRes_BookExchangePost>();

            var bookExchange = await _dbContext.BookExchanges
                .Include(x => x.BookExchangeDetails)
                .FirstOrDefaultAsync(x => x.Id == request.BookExchangeId);
            if (bookExchange == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            // Check exchangeable quantity exceeds created quantity
            if (request.ExchangeableQuantity > bookExchange.CreatedQuantity)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = ["Số lượng trao đổi không được vượt quá số lượng sách"];
                return response;
            }

            var data = _mapper.Map<BookExchangePost>(request);
            data.CreatedAt = DateTime.Now;

            // Update book exchange details status to "Exchangeable"
            bookExchange.BookExchangeDetails
                .Where(x => x.Status == (byte)EN_BookExchangeDetail.Status.Created)
                .OrderBy(x => x.Id)
                .Take(data.ExchangeableQuantity)
                .ToList()
                .ForEach(x => x.Status = (byte)EN_BookExchangeDetail.Status.Exchangeable);

            // Update book exchange created quantity
            bookExchange.CreatedQuantity = bookExchange.BookExchangeDetails
                .Where(x => x.Status == (byte)EN_BookExchangeDetail.Status.Created)
                .Count();

            _dbContext.BookExchangePosts.Add(data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status201Created;
            response.Data = _mapper.Map<MRes_BookExchangePost>(data);
            response.Message = [MessageErrorConstant.CREATE_SUCCESS];
            return response;
        }
    }
}
