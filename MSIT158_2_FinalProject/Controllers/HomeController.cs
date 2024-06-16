using Microsoft.AspNetCore.Mvc;
using MSIT158_2_FinalProject.Models;
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
            // �ˬd Session ���O�_�s�b�n���|�������
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                // ���o Session ���
                var loginMemberData = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                // �N JSON �r��ϧǦC�Ƭ��ʪ����C��
                TMember User = JsonSerializer.Deserialize<TMember>(loginMemberData);
                // �ϥ� ViewBag �N Session ��ƶǻ������
                ViewBag.LoginMember = loginMemberData;
                // �p�G�s�b�A�h��ܭ�������
                return View(User);
            }
            // �p�G���s�b�A�h�ɦV�n������
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
        //��x�n�J
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
        //�إ߷s�b��
        public IActionResult MemberCreate()
        {
            return View();
        }
        //�ѰO�K�X�A�T�{�H�c��H�eemail�A�bemail�ק�K�X��e�^API��K�X�AAPI�H�eemail���\�q��
        public IActionResult MemberForgetPassword()
        {
            return View();
        }
        //google �ĤT��n�J(API)
        public IActionResult GoogleAuth()
        {
            return View();
        }
        //FB �ĤT��n�J(API)
        public IActionResult FacebookAuth()
        {
            return View();
        }
        //��ɴ���
        public IActionResult CashFlow()
        {
    //        int cash = 1000;
    //        string productname = "�v����-���հӫ~��A #�v����-���հӫ~��B #�v����-���հӫ~��C";

    //        var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
    //        //�ݶ�J�A�����}
    //        var website = $"https://localhost:7066/";
    //        var order = new Dictionary<string, string>
    //{
    //    //��ɻݭn���Ѽ�
    //    { "MerchantTradeNo",  orderId},
    //    { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
    //    { "TotalAmount",  $"{cash}"},
    //    { "TradeDesc",  "�L"},
    //    { "ItemName",  productname },
    //    //{ "ExpireDate",  "3"},
    //    //{ "CustomField1",  "�ӫ~AA"},
    //    //{ "CustomField2",  "�ӫ~BB"},
    //    //{ "CustomField3",  ""},
    //    //{ "CustomField4",  ""},
    //    { "ReturnURL",  $"{website}/Home/CashFlow"},
    //    //{ "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
    //    { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
    //    //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
    //    { "MerchantID",  "3002607"},
    //    { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
    //    { "PaymentType",  "aio"},
    //    { "ChoosePayment",  "ALL"},
    //    { "EncryptType",  "1"},
    //};
    //        //�ˬd�X
    //        order["CheckMacValue"] = new CCheckMacValue().GetCheckMacValue(order);
    //        //return Ok(order);
            return View();
        }
        public IActionResult CashFlowB()
        {
            return View();
        }
        //private string GetCheckMacValue(Dictionary<string, string> order)
        //{
        //    var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
        //    var checkValue = string.Join("&", param);
        //    //���եΪ� HashKey
        //    var hashKey = "pwFHCqoQZGmho4w6";
        //    //���եΪ� HashIV
        //    var HashIV = "EkRm7iFT261dpevs";
        //    checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
        //    checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
        //    checkValue = GetSHA256(checkValue);
        //    return checkValue.ToUpper();
        //}
        //private string GetSHA256(string value)
        //{
        //    var result = new StringBuilder();
        //    var sha256 = SHA256.Create();
        //    var bts = Encoding.UTF8.GetBytes(value);
        //    var hash = sha256.ComputeHash(bts);
        //    for (int i = 0; i < hash.Length; i++)
        //    {
        //        result.Append(hash[i].ToString("X2"));
        //    }
        //    return result.ToString();
        //}



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
