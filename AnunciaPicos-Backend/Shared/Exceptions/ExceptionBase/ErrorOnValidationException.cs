namespace AnunciaPicos.Exceptions.ExceptionBase
{
    public class ErrorOnValidationException : AnunciaPicosExceptions
    {
        public IList<string> ErrorsMessages { get; set; }

        public ErrorOnValidationException(IList<string> errorsMessages) : base(string.Empty)
        {
            ErrorsMessages = errorsMessages;
        }

    }
}
