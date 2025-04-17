using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Response.Product;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Product
{
    public class GetProductMapping : AutoMapper.Profile
    {
        public GetProductMapping()
        {
            CreateMap<ProductModel, ResponseProductGetCommunication>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));


        }
    }
}
