using AutoMapper;
using BookTradeAPI.Data;
using BookTradeAPI.Models.Common;
using BookTradeAPI.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace BookTradeAPI.Services
{
    public interface IS_BookExchangePost
    {
        Task<ApiResponse<List<MRes_BookExchangePost>>> GetAll();
    }
    public class S_BookExchangePost : IS_BookExchangePost
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public S_BookExchangePost(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<MRes_BookExchangePost>>> GetAll()
        {
            var response = new ApiResponse<List<MRes_BookExchangePost>>();

            var data = await _dbContext.BookExchangePosts
                .Include(x => x.BookExchange)
                .AsNoTracking()
                .ToListAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Data = _mapper.Map<List<MRes_BookExchangePost>>(data);
            return response;
        }
    }
}
