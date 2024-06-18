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

        public IActionResult SenderRegisterEmail()
        {
            string receive = "k955339962@gmail.com";
            string subject = "*** 用戶註冊驗證";
            string messages = "<h1>歡迎註冊***</h1>";
            messages += "<p>請點擊以下連結驗證您的帳號:</p>";
            messages += "<a>點擊這裡</a>進行驗證";
            new CEmailSender().getEmail(receive,subject,messages);

            return Ok("郵件已成功發送");
        }
        public IActionResult SenderForgetPasswordEmail()
        {
            //如果沒有密碼，將無法寄信修改密碼
            string receive = "fetch的信箱";
            string subject = "*** 用戶重新設定密碼";
            string messages = "<h1>修改***的密碼</h1>";
            messages += "<p>請點擊以下連結修改您的密碼:</p>";
            messages += "<a>點擊這裡</a>進行驗證";
            new CEmailSender().getEmail(receive, subject, messages);

            return Ok("郵件已成功發送");
        }
        //綠界金流API
        public IActionResult CashFlow(int totalAmount, string itemName)
        {
            //int cash = 1000;
            //string productnames = "史先生-測試商品中A #史先生-測試商品中B #史先生-測試商品中C";

















            //==================================================
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://localhost:7066";
            var order = new Dictionary<string, string>
    {
        //綠界需要的參數
        { "MerchantTradeNo",  orderId},
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
        { "TotalAmount",  $"{totalAmount}"},
        { "TradeDesc",  "無"},
        { "ItemName",  itemName },
        { "ReturnURL",  $"{website}/Home/CashFlow"},
        //{ "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
        { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
        //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
        { "ClientBackURL",  $"{website}/Home/CashFlowB"},
        { "MerchantID",  "3002607"},
        { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
        { "PaymentType",  "aio"},
        { "ChoosePayment",  "ALL"},
        { "EncryptType",  "1"},
    };
            //檢查碼
            order["CheckMacValue"] = new CCheckMacValue().GetCheckMacValue(order);

            return Ok(order);
        }
    }
}
