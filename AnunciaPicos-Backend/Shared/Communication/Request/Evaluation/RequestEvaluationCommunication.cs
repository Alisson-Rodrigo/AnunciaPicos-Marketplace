namespace AnunciaPicos.Shared.Communication.Request.Evaluation
{
    public class RequestEvaluationCommunication
    {
        public int UserIdEvaluated { get; set; }  // ID do usuário que está sendo avaliado

        public int Note { get; set; }  // Nota da avaliação (geralmente entre 1 e 5)

        public string? Comment { get; set; }  // Comentário da avaliação, pode ser nulo
    }
}
