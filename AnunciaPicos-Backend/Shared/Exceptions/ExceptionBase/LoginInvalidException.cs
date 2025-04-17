using AnunciaPicos.Exceptions;
using AnunciaPicos.Shared.Exceptions;

namespace AnunciaPicos.Exceptions.ExceptionBase
{
    public class LoginInvalidException : AnunciaPicosExceptions
    {
        public LoginInvalidException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
        {
        }
    }
}
