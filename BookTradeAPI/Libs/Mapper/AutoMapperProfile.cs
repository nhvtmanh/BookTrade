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

            CreateMap<MReq_Category, Category>().ReverseMap();
            CreateMap<MRes_Category, Category>().ReverseMap();

            CreateMap<MReq_User_Register, User>().ReverseMap();
            CreateMap<MReq_User_Login, User>().ReverseMap();
            CreateMap<MRes_User, User>().ReverseMap();

            CreateMap<MReq_Seller_Register, Shop>().ReverseMap();
            CreateMap<MRes_Shop, Shop>().ReverseMap();

            CreateMap<MRes_Cart, Cart>().ReverseMap();

            CreateMap<MReq_Notification, Notification>().ReverseMap();
            CreateMap<MRes_Notification_Detail, Notification>().ReverseMap();
        }
    }
}
