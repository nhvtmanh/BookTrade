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
        Task<ApiResponse<MRes_BookExchange>> Update(MReq_BookExchange request);
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
            data.BookExchangeDetails = Enumerable.Range(1, data.CreatedQuantity)
                .Select(x => new BookExchangeDetail
                {
                    Status = (byte)EN_BookExchangeDetail.Status.Created
                })
                .ToList();

            // Set book exchange quantity
            data.Quantity = data.CreatedQuantity;

            _dbContext.BookExchanges.Add(data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status201Created;
            response.Data = _mapper.Map<MRes_BookExchange>(data);
            response.Message = [MessageErrorConstant.CREATE_SUCCESS];
            return response;
        }

        public async Task<ApiResponse<MRes_BookExchange>> Update(MReq_BookExchange request)
        {
            var response = new ApiResponse<MRes_BookExchange>();

            var bookExchange = await _dbContext.BookExchanges
                .Include(x => x.BookExchangeDetails)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (bookExchange == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            if (bookExchange.CreatedQuantity > request.CreatedQuantity)
            {
                int deleteQuantity = bookExchange.CreatedQuantity - request.CreatedQuantity;
                // Delete book exchange details with status "Created"
                var deleteBookExchangeDetails = bookExchange.BookExchangeDetails
                    .Where(x => x.Status == (byte)EN_BookExchangeDetail.Status.Created)
                    .OrderByDescending(x => x.Id)
                    .Take(deleteQuantity)
                    .ToList();
                foreach (var item in deleteBookExchangeDetails)
                {
                    bookExchange.BookExchangeDetails.Remove(item);
                }
            }
            else if (bookExchange.CreatedQuantity < request.CreatedQuantity)
            {
                int createQuantity = request.CreatedQuantity - bookExchange.CreatedQuantity;
                // Create book exchange details with status "Created"
                var createBookExchangeDetails = Enumerable.Range(1, createQuantity)
                    .Select(x => new BookExchangeDetail
                    {
                        Status = (byte)EN_BookExchangeDetail.Status.Created,
                        BookExchangeId = bookExchange.Id
                    })
                    .ToList();
                foreach (var item in createBookExchangeDetails)
                {
                    bookExchange.BookExchangeDetails.Add(item);
                }
            }

            _mapper.Map(request, bookExchange);

            // Update book exchange quantity
            bookExchange.Quantity = bookExchange.BookExchangeDetails.Count();

            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_BookExchange>(bookExchange);
            response.Message = [MessageErrorConstant.UPDATE_SUCCESS];
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

            // Update book exchange exchangeable quantity
            bookExchange.ExchangeableQuantity = bookExchange.BookExchangeDetails
                .Where(x => x.Status == (byte)EN_BookExchangeDetail.Status.Exchangeable)
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
