using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AnunciaPicos.Exceptions.ExceptionBase;
using AnunciaPicos.Shared.Exceptions;
using AnunciaPicos.Shared.Communication.Response;

namespace AnunciaPicos.Backend.API.Filters
{
    public class ExceptionsFilter : IExceptionFilter
    {
        // Função que é chamada quando ocorre uma exceção.
        public void OnException(ExceptionContext context)
        {
            // Verificar se a exceção é do tipo MyRecipesBookExceptions
            if (context.Exception is AnunciaPicosExceptions)
            {
                HandleProjectException(context);
            }
            // Se não for, então é um erro desconhecido.
            else
            {
                ThrowUnknowException(context);
            }
        }

        // Nessa função é tratado a exceção do projeto e retornando o erro para o cliente.
        private void HandleProjectException(ExceptionContext context)
        {

            if (context.Exception is LoginInvalidException)
            {

                //Retornar um BadRequest com a lista de erros
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new UnauthorizedObjectResult(new ResponseErrosJson(context.Exception.Message));
            }

            // Se a exceção for do tipo ErrorOnValidationException, então é retornado um BadRequest com a lista de erros.
            else if (context.Exception is ErrorOnValidationException)
            {
                //Pegar a exceção e transformar em um objeto do tipo ErrorOnValidationException
                var exception = context.Exception as ErrorOnValidationException;

                //Retornar um BadRequest com a lista de erros
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrosJson(exception!.ErrorsMessages));
            }

            else if (context.Exception is AnunciaPicosExceptions exception)
            {
                // Você pode capturar a mensagem da exceção diretamente
                var message = string.IsNullOrEmpty(exception.Message) ? "An error occurred" : exception.Message;

                // Aqui, você pode decidir qual status HTTP retornar e incluir a mensagem no corpo da resposta
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrosJson(message));
            }
        }

        private void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //Retornar um BadRequest com a lista de erros
            Console.WriteLine(context.Exception.Message);
            context.Result = new ObjectResult(new ResponseErrosJson(ResourceMessagesException.UNKNOW_ERROR));
        }
    }
}
