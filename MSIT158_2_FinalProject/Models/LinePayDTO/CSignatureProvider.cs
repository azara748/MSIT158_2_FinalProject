using System.Security.Cryptography;

namespace MSIT158_2_FinalProject.Models.LinePayDTO
{
    public class CSignatureProvider
    {
        public string HMACSHA256(string key, string message)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            //取的 key byte 值
            byte[] keyByte = encoding.GetBytes(key);

            // 取得 key 對應的 hmacsha256
            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            // 取的 message byte 值
            byte[] messageBytes = encoding.GetBytes(message);

            // 將 message 使用 key 值對應的 hamcsha256 作 hash 簽章
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

            return Convert.ToBase64String(hashmessage);
        }
    }
}
