using AnunciaPicos.Backend.Infrastructure.Enums;

namespace AnunciaPicos.Shared.Communication.Request.Auth
{
    public class RequestRegisterCommunication
    {
        // Mapeia para UserModel.Name
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}
