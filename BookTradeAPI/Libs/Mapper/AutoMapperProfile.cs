using AutoMapper;
using BookTradeAPI.Models.Entities;
using BookTradeAPI.Models.Request;
using BookTradeAPI.Models.Response;

namespace BookTradeAPI.Libs.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MReq_BookExchange, BookExchange>().ReverseMap();
            CreateMap<MRes_BookExchange, BookExchange>().ReverseMap();

            CreateMap<MReq_BookExchangePost, BookExchangePost>().ReverseMap();
            CreateMap<MRes_BookExchangePost, BookExchangePost>().ReverseMap();
        }
    }
}
