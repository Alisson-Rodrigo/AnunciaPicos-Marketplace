namespace AnunciaPicos.Shared.Communication.Response.Evaluation
{
    public class ResponseGetEvaluationCommunicattion
    {
        public int Id { get; set; }
        public string UserImage { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Note { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}
