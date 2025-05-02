using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Response.Profile;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Profile
{
    public class GetUserProfileMapping : AutoMapper.Profile
    {
        public GetUserProfileMapping()
        {
            CreateMap<UserModel, ResponseGetProfileCommunication>()
                .ForMember(dest => dest.ImageProfile, opt => opt.MapFrom(src => src.ImageProfile))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));

            CreateMap<UserModel, ResponseGetProfileIdCommunication>()
                .ForMember(dest => dest.ImageProfile, opt => opt.MapFrom(src => src.ImageProfile))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));
        }
    }
}
