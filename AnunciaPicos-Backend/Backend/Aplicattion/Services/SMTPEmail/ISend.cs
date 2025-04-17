namespace AnunciaPicos.Backend.Aplicattion.Services.SMTPEmail
{
    public interface ISend
    {
        public bool SendRecoveryEmail(string recipientEmail, string recoveryLink);

    }
}
