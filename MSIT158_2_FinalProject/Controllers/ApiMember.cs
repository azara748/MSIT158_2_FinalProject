using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.ViewModel;
using System.Text.Json;

namespace MSIT158_2_FinalProject.Controllers
{
    public class ApiMember : Controller
    {
        private readonly SelectShopContext _context;
        public ApiMember(SelectShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(CLoginViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(
                t => t.EMail.Equals(vm.txtEmail) && t.Password.Equals(vm.txtPassword));
            string json = null;
            if (user != null && user.Password.Equals(vm.txtPassword))
            {
                json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);
            }
            return Content(json, "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult OauthLogin([FromBody] COauthLoginViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(
                t => t.EMail.Equals(vm.txtOauthEmail) && t.MemberName.Equals(vm.txtOauthName));
            string json = null;
            if (user != null && user.EMail.Equals(vm.txtOauthEmail))
            {
                json = JsonSerializer.Serialize(user);
                HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);
            }
            return Content(json, "text/plain", System.Text.Encoding.UTF8);
        }
        public IActionResult AddSession()
        {
            // 取得 Session 資料
            var loginMemberData = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
            // 將 JSON 字串反序列化為購物車列表
            TMember User = JsonSerializer.Deserialize<TMember>(loginMemberData);

            return Content(loginMemberData, "text/plain", System.Text.Encoding.UTF8);
        }
        public IActionResult ClearSession()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGIN_MEMBER);
            return Ok("Session 已清除。");
        }

        public IActionResult SenderEmail()
        {
            string receive = "k955339962@gmail.com";
            string subject = "*** 用戶註冊驗證";
            string messages = "<h1>歡迎註冊***</h1>";
            messages += "<p>請點擊以下連結驗證您的帳號:</p>";
            messages += "<a>點擊這裡</a>進行驗證";
            new CEmailSender().getEmail(receive,subject,messages);

            return Ok("郵件已成功發送");
        }
    }
}
