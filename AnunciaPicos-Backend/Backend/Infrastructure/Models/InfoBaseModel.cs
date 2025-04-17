namespace AnunciaPicos.Backend.Infrastructure.Models
{
    public class InfoBaseModel
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool Active { get; set; } = true;

    }
}
