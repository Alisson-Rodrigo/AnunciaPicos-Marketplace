namespace AnunciaPicos.Shared.Communication.Request.Auth
{
    public class RequestResetPasswordCommunication
    {
        public string Token { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
