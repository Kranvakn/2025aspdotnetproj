
using System.Security.Cryptography;
using System.Text;

namespace todoApp.Utils
{
    public static class Hash
    {
        public static string setHashPassword(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData)); // 문자열을 바이트 배열로 변환 
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2")); // 16진수로 변환
            }
            return builder.ToString(); // 해시값 반환
        }
    }
}