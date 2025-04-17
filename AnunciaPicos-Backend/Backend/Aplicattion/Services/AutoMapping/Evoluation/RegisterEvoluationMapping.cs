using AnunciaPicos.Shared.Communication.Request.Evaluation;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Response.Evaluation;

namespace AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Evoluation
{
    public class RegisterEvoluationMapping : AutoMapper.Profile
    {
        public RegisterEvoluationMapping()
        {
            CreateMap<RequestEvaluationCommunication, EvaluationModel>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))  // Mapeia a Nota
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment));  // Mapeia o Comentário

        }
    }
}
