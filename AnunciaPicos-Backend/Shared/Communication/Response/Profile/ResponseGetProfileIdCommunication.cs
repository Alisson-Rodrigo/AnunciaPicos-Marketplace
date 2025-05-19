namespace AnunciaPicos.Shared.Communication.Response.Profile
{
    public class ResponseGetProfileIdCommunication
    {
        public int Id { get; set; }
        public string ImageProfile { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
