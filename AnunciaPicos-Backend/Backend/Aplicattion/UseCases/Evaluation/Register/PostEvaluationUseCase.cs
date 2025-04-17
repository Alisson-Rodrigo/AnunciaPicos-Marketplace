using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Infrastructure.Models;
using AnunciaPicos.Backend.Infrastructure.Repositories.Evaluation;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Communication.Request.Evaluation;
using AnunciaPicos.Shared.Exceptions;
using AutoMapper;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Register
{
    public class PostEvaluationUseCase : IPostEvaluationUseCase
    {
        private readonly ILogged _loggedUser;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostEvaluationUseCase(ILogged loggedUser, IUserRepository userRepository, IMapper mapper, IEvaluationRepository evaluationRepository, IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _userRepository = userRepository;
            _mapper = mapper;
            _evaluationRepository = evaluationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestEvaluationCommunication request)
        {
            UserModel user = await _loggedUser.UserLogged();
            UserModel userEvaluation = await _userRepository.GetUserById(request.UserIdEvaluated);

            //validar a requesição
            if (userEvaluation != null)
            {
                await ValidationEvaluation(request, user, userEvaluation);
            }
            else {
                throw new AnunciaPicosExceptions(ResourceMessagesException.SELLER_NOT_FOUND);
            }

            //fazer o mapeamento
            var evaluation = _mapper.Map<EvaluationModel>(request);

            evaluation.User = user;
            evaluation.UserEvaluated = userEvaluation;
            try
            {
                //cadastrar no bd
                await _evaluationRepository.RegisterEvaluation(evaluation);
                //sakvar no banco
                await _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new AnunciaPicosExceptions(ResourceMessagesException.UNKNOW_ERROR);
            }

        }

        private async Task ValidationEvaluation(RequestEvaluationCommunication request, UserModel user, UserModel userEvaluation)
        {

            var validator = new PostEvaluationValidation();

            var result = validator.Validate(request);

            if (user!.Id == request.UserIdEvaluated)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EVALUATION_NOT_ALLOWED));
            }

            //verificar se o usuario ja postou algum comentario sobre esse vendedor
            var userCommentSeller = await _evaluationRepository.VerifyUserCommentSeller(user.Id, userEvaluation!.Id);

            if (userCommentSeller)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EXISTING_EVALUATION));
            }


            if (!result.IsValid)
                {
                    //Pegar a mensagem de erro e lançar uma exceção
                    var errorMessage = result.Errors.Select(e => e.ErrorMessage).ToList();
                    //Lançar uma exceção
                    throw new ErrorOnValidationException(errorMessage);
                }
        }

    }
}
