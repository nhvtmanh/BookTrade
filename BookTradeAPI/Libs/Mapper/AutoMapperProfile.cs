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
            CreateMap<MReq_Book, Book>().ReverseMap();
            CreateMap<MRes_Book, Book>().ReverseMap();

            CreateMap<MReq_BookExchange, BookExchange>().ReverseMap();
            CreateMap<MRes_BookExchange, BookExchange>().ReverseMap();

            CreateMap<MReq_BookExchangePost, BookExchangePost>().ReverseMap();
            CreateMap<MRes_BookExchangePost, BookExchangePost>().ReverseMap();

            CreateMap<MReq_Category, Category>().ReverseMap();
            CreateMap<MRes_Category, Category>().ReverseMap();
        }
    }
}
