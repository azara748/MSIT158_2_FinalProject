using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace MSIT158_2_API.Models
{
    public class CEmailSender 
    {
        static void Main(string[] args)
        {
            // 寄件者的信箱和密碼
            string fromMail = "k955339962@gmail.com";
            string fromPassword = "tszbirukriblptqz";

            // 建立郵件訊息物件
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail); // 設定寄件者
            message.Subject = "Test Subject"; // 設定郵件主題
            message.To.Add(new MailAddress("k955339962@gmail.com")); // 加入收件者
            message.Body = "<html><body> Test Body*** </body></html>"; // 設定郵件內容，這裡假設是 HTML 格式
            message.IsBodyHtml = true; // 指定內容為 HTML 格式

            // 建立 SMTP 客戶端並設定參數
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // Gmail SMTP 的埠號
                Credentials = new NetworkCredential(fromMail, fromPassword), // 設定 SMTP 認證資訊
                EnableSsl = true,// 啟用 SSL 加密連線
            };

            // 發送郵件
            smtpClient.Send(message);
        }
    }
}
