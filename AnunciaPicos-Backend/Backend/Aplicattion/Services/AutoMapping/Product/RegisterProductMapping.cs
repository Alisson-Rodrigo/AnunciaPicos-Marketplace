using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Request.Product;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Product
{
    public class RegisterProductMapping : AutoMapper.Profile
    {
        public RegisterProductMapping()
        {
            CreateMap<RequestRegisterProductCommunication, ProductModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Categories));
        } 
    }
}
