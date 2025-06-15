namespace AnunciaPicos.Shared.Communication.Response.Auth
{
    public class ResponseFacebookLoginCommunication
    {
        public string Token { get; set; } = string.Empty;
        public UserInfoResponse User { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }

    public class UserInfoResponse
    {
        public Guid UserIdentifier { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageProfile { get; set; }
        public string Provider { get; set; } = string.Empty;
        public bool IsSocialLogin { get; set; }
    }
}
