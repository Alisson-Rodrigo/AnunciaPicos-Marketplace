namespace AnunciaPicos.Backend.Aplicattion.Services.HashPassword
{
    public interface IPasswordEncrypter
    {
        public string Encript(string password, string _additionalKey);


    }
}
