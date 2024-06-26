using Microsoft.AspNetCore.Mvc;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.Models.前台;
using MSIT158_2_FinalProject.ViewModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace MSIT158_2_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SelectShopContext _context;

        public HomeController(ILogger<HomeController> logger, SelectShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // 檢查 Session 中是否存在登錄會員的資料
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                // 取得 Session 資料
                var loginMemberData = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                // 將 JSON 字串反序列化為購物車列表
                TMember User = JsonSerializer.Deserialize<TMember>(loginMemberData);
                // 使用 ViewBag 將 Session 資料傳遞到視圖
                ViewBag.LoginMember = loginMemberData;
                // 如果存在，則顯示首頁視圖
                return View(User);
            }
            // 如果不存在，則導向登錄頁面
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm)
        {
            //TMember user = _context.TMembers.FirstOrDefault(
            //    t => t.EMail.Equals(vm.txtEmail) && t.Password.Equals(vm.txtPassword));

            //if (user != null && user.Password.Equals(vm.txtPassword))
            //{
            //    string json = JsonSerializer.Serialize(user);
            //    HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);

            //    return RedirectToAction("Index");
            //}
            return View();
        }
        //後台登入
        public IActionResult BackIndex()
        {
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_EMPLOYEE))
            //    return View();
            //return RedirectToAction("BackLogin");
                return View();
        }

        public IActionResult BackLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BackLogin(CLoginViewModel vm)
        {
            //TEmployee emp = _context.TEmployees.FirstOrDefault(
            //    t => t.EMail.Equals(vm.txtEmail) && t.Password.Equals(vm.txtPassword));
            //TEmployee x = _context.TEmployees.FirstOrDefault(x=>x.EMail.Equals(vm.txtEmail));

            //if (emp != null && emp.Password.Equals(vm.txtPassword))
            //{
            //    string json = JsonSerializer.Serialize(emp);
            //    HttpContext.Session.SetString(CDictionary.SK_LOGIN_EMPLOYEE, json);

            //    return RedirectToAction("BackIndex");
            //}
            return View();
        }
        //建立新帳號
        public IActionResult MemberCreate()
        {
            return View();
        }
        //忘記密碼，確認信箱後寄送email，在email修改密碼後送回API改密碼，API寄送email成功通知
        public IActionResult MemberForgetPassword()
        {
            return View();
        }
        public IActionResult MemberForgetPasswordB()
        {
            return View();
        }
        //google 第三方登入(API)
        public IActionResult GoogleAuth()
        {
            return View();
        }
        //FB 第三方登入(API)
        public IActionResult FacebookAuth()
        {
            return View();
        }
        //綠界測試
        public IActionResult CashFlow(int totalAmount,int oid)
        {
            List<string> fields = new List<string>();
            fields = new fM結帳().明細(oid);
            string productname = null;
            foreach (var field in fields)
            {
                productname += field.ToString();
                productname += " #";
            }
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);


            // 修改訂單狀態 將StatusID改成2
            var orderToUpdate = _context.TOrders.FirstOrDefault(o => o.OrderId == oid);
            if (orderToUpdate != null)
            {
                orderToUpdate.StatusId = 2;
                orderToUpdate.MerchantTradeNo = orderId;
                _context.SaveChanges();
            }
            //需填入你的網址
            var website = $"https://localhost:7066";
            var Apiweb = "https://localhost:7160";
            var ngrok = "https://5e0e-1-160-2-62.ngrok-free.app";
            var order = new Dictionary<string, string>
    {
        //綠界需要的參數
        { "MerchantTradeNo",  orderId},
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
        { "TotalAmount",  $"{totalAmount}"},
        { "TradeDesc",  "無"},
        { "ItemName",  productname },
        //{ "ExpireDate",  "3"},
        //{ "CustomField1",  "商品AA"},
        //{ "CustomField2",  "商品BB"},
        //{ "CustomField3",  ""},
        //{ "CustomField4",  ""},
        { "ReturnURL",  $"{ngrok}/api/TMembers/ReceiveCashFlow"},
        //{ "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
        { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
        //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
        { "ClientBackURL",  $"{website}/M/Memberpage"},
        { "MerchantID",  "3002607"},
        { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
        { "PaymentType",  "aio"},
        { "ChoosePayment",  "ALL"},
        { "EncryptType",  "1"},
    };
            //檢查碼
            order["CheckMacValue"] = new CCheckMacValue().GetCheckMacValue(order);
            return View(order);
        }
        public IActionResult CashFlowB()
        {
            return View();
        }
        



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }       
    }
}
