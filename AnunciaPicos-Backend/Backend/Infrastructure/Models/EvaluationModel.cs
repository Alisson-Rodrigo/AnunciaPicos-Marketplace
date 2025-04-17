namespace AnunciaPicos.Backend.Infrastructure.Models
{
    public class EvaluationModel
    {
        public int Id { get; set; } 

        public int UserId { get; set; }  

        public int UserIdEvaluated { get; set; } 

        public int Note { get; set; }  

        public string? Comment { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;  

        public DateTime UpdatedAt { get; set; } = DateTime.Now;  

        public UserModel? User { get; set; }

        public UserModel? UserEvaluated { get; set; }
    }
}
