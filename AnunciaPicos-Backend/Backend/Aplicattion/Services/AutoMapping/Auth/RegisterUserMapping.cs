using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Request.Auth;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Auth
{
    public class RegisterUserMapping : AutoMapper.Profile
    {
        public RegisterUserMapping()
        {
            CreateMap<RequestRegisterCommunication, UserModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));
        }

    }
}
