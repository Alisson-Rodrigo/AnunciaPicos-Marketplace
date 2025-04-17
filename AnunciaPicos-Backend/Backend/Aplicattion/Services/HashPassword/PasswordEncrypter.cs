using System.Security.Cryptography;
using System.Text;

namespace AnunciaPicos.Backend.Aplicattion.Services.HashPassword
{
    public class PasswordEncrypter : IPasswordEncrypter
    {
        public string Encript(string password, string _additionalKey)
        {
            var newPass = $"{password}{_additionalKey}";

            var bytes = new UTF8Encoding().GetBytes(newPass);
            var hashBytes = SHA512.HashData(bytes);
            return ConvertToString(hashBytes);
        }

        //convertendo array de bytes para string
        private static string ConvertToString(byte[] hashBytes)
        {

            var builder = new StringBuilder();
            foreach (var b in hashBytes)
            {
                var hex = b.ToString("x2");
                builder.Append(hex);
            }
            return builder.ToString();
        }
    }
}
