namespace AnunciaPicos.Shared.Communication.Response
{
    public class ResponseErrosJson
    {
        public IList<string> Errors { get; set; }
        public bool TokenIsExpired { get; set; }

        public ResponseErrosJson(IList<string> errors)
        {
            Errors = errors;
        }

        public ResponseErrosJson(string error)
        {
            Errors = new List<string> { error };
        }
    }
}
