using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Evaluation;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Response.Evaluation;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;
using Azure;
using Google.Protobuf.WellKnownTypes;
using Mysqlx.Prepare;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Get
{
    public class GetEvaluationUseCase : IGetEvaluationUseCase
    {

        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetEvaluationUseCase(IEvaluationRepository evaluationRepository, IMapper mapper, IUserRepository userRepository)
        {
            _evaluationRepository = evaluationRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<ResponseGetEvaluationCommunicattion>> Execute (int id)
        {
            var verifyUserEvaluationExists = await _userRepository.GetUserById(id);

            if (verifyUserEvaluationExists == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_NOT_FOUND);
            }
            
            List<EvaluationModel> evaluation = await _evaluationRepository.GetEvaluationSeller(id);

            if (!evaluation.Any()) {
                throw new AnunciaPicosExceptions(ResourceMessagesException.EVALUATION_NOT_EMPTY);
            }

            var evaluationMap = _mapper.Map<List<ResponseGetEvaluationCommunicattion>>(evaluation);


            // Neste ponto, iremos colocar o nome e a imagem de cada pessoa que fez a avaliaçao
            // Adiciona nome e imagem do avaliador
            foreach (var evaluationItem in evaluationMap)
            {
                // Encontra a avaliação correspondente no modelo original
                var model = evaluation.FirstOrDefault(e => e.Id == evaluationItem.Id);

                if (model != null)
                {
                    var user = await _userRepository.GetUserById(model.UserId);
                    if (user != null)
                    {
                        evaluationItem.Name = user.Name; // ou Nome, dependendo do campo
                        evaluationItem.UserImage = user.ImageProfile ?? string.Empty; // ou outro campo equivalente
                    }
                }
            }


            return evaluationMap;
        }

        public async Task<double> ExecuteAverage (int id)
        {
            var verifyUserEvaluationExists = await _userRepository.GetUserById(id);

            if (verifyUserEvaluationExists == null)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.USER_NOT_FOUND);
            }

            double average = await _evaluationRepository.GetAverageNoteById(id);
            return average;

        }
    }
}
