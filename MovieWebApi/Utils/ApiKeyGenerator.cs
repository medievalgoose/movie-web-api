using System.Security.Cryptography;
using System.Text;

namespace MovieWebApi.Utils
{
    public class ApiKeyGenerator
    {
        public static string GenerateSHA256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    // X2 means that the string should be formatted in Hexadecimal.
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
