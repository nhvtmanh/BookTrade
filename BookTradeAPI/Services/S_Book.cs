using AutoMapper;
using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Utilities.Constants;
using BookTradeAPI.Utilities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookTradeAPI.Services
{
    public interface IS_Book
    {
        Task<ApiResponse<List<MRes_Book>>> GetAll();
        Task<ApiResponse<MRes_Book>> GetById(int id);
        Task<ApiResponse<MRes_Book>> Create(MReq_Book request);
        Task<ApiResponse<MRes_Book>> Update(MReq_Book request);
        Task<ApiResponse<MRes_Book>> Delete(int id);
    }
    public class S_Book : IS_Book
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public S_Book(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<MRes_Book>>> GetAll()
        {
            var response = new ApiResponse<List<MRes_Book>>();

            var data = await _dbContext.Books
                .AsNoTracking()
                .Include(x => x.Shop)
                .ToListAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<List<MRes_Book>>(data);
            return response;
        }

        public async Task<ApiResponse<MRes_Book>> GetById(int id)
        {
            var response = new ApiResponse<MRes_Book>();

            var data = await _dbContext.Books.FindAsync(id);
            if (data == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_Book>(data);
            return response;
        }

        public async Task<ApiResponse<MRes_Book>> Create(MReq_Book request)
        {
            var response = new ApiResponse<MRes_Book>();

            var data = _mapper.Map<Book>(request);
            data.Status = (byte)EN_Book.Status.Available;
            data.CreatedAt = DateTime.Now;

            var shop = await _dbContext.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId);
            data.ShopId = shop!.Id;

            _dbContext.Books.Add(data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status201Created;
            response.Data = _mapper.Map<MRes_Book>(data);
            response.Message = [MessageErrorConstant.CREATE_SUCCESS];
            return response;
        }

        public async Task<ApiResponse<MRes_Book>> Update(MReq_Book request)
        {
            var response = new ApiResponse<MRes_Book>();

            var data = await _dbContext.Books.FindAsync(request.Id);
            if (data == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            _mapper.Map(request, data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_Book>(data);
            response.Message = [MessageErrorConstant.UPDATE_SUCCESS];
            return response;
        }

        public async Task<ApiResponse<MRes_Book>> Delete(int id)
        {
            var response = new ApiResponse<MRes_Book>();

            var data = await _dbContext.Books.FindAsync(id);
            if (data == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            _dbContext.Books.Remove(data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_Book>(data);
            response.Message = [MessageErrorConstant.DELETE_SUCCESS];
            return response;
        }
    }
}
