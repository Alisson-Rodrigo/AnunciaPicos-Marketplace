using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Shared.Communication.Response.Evaluation;

namespace AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Evoluation
{
    public class GetEvaluationMapping : AutoMapper.Profile
    {
        public GetEvaluationMapping()
        {
            CreateMap<EvaluationModel, ResponseGetEvaluationCommunicattion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
