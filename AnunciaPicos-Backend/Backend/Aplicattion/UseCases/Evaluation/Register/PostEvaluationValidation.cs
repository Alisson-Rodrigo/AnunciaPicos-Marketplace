using AnunciaPicos.Shared.Communication.Request.Evaluation;
using FluentValidation;

namespace AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Register
{
    public class PostEvaluationValidation : AbstractValidator<RequestEvaluationCommunication>
    {
        public PostEvaluationValidation() { 
            //verificar se nao é vazio, se esta entre 1 e 5
            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("A nota não pode ser vazia.")
                .InclusiveBetween(1, 5).WithMessage("A nota deve estar entre 1 e 5.");
        }
    }
}
