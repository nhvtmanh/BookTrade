using AutoMapper;
using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;
using BookTradeAPI.Utilities.Constants;
using Microsoft.EntityFrameworkCore;

namespace BookTradeAPI.Services
{
    public interface IS_Category
    {
        Task<ApiResponse<List<MRes_Category>>> GetAll();
        Task<ApiResponse<MRes_Category>> GetById(int id);
        Task<ApiResponse<MRes_Category>> Create(MReq_Category request);
        Task<ApiResponse<MRes_Category>> Update(MReq_Category request);
        Task<ApiResponse<MRes_Category>> Delete(int id);
    }
    public class S_Category : IS_Category
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public S_Category(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<MRes_Category>>> GetAll()
        {
            var response = new ApiResponse<List<MRes_Category>>();

            var data = await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<List<MRes_Category>>(data);
            return response;
        }

        public async Task<ApiResponse<MRes_Category>> GetById(int id)
        {
            var response = new ApiResponse<MRes_Category>();

            var data = await _dbContext.Categories.FindAsync(id);
            if (data == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_Category>(data);
            return response;
        }

        public async Task<ApiResponse<MRes_Category>> Create(MReq_Category request)
        {
            var response = new ApiResponse<MRes_Category>();

            var data = _mapper.Map<Category>(request);

            _dbContext.Categories.Add(data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status201Created;
            response.Data = _mapper.Map<MRes_Category>(data);
            response.Message = [MessageErrorConstant.CREATE_SUCCESS];
            return response;
        }

        public async Task<ApiResponse<MRes_Category>> Update(MReq_Category request)
        {
            var response = new ApiResponse<MRes_Category>();

            var data = await _dbContext.Categories.FindAsync(request.Id);
            if (data == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            _mapper.Map(request, data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_Category>(data);
            response.Message = [MessageErrorConstant.UPDATE_SUCCESS];
            return response;
        }

        public async Task<ApiResponse<MRes_Category>> Delete(int id)
        {
            var response = new ApiResponse<MRes_Category>();

            var data = await _dbContext.Categories.FindAsync(id);
            if (data == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = [MessageErrorConstant.NOT_FOUND];
                return response;
            }

            _dbContext.Categories.Remove(data);
            await _dbContext.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<MRes_Category>(data);
            response.Message = [MessageErrorConstant.DELETE_SUCCESS];
            return response;
        }
    }
}
