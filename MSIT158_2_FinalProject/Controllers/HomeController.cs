using Microsoft.AspNetCore.Mvc;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.ViewModel;
using System.Diagnostics;
using System.Text.Json;

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
        //�ѰO�K�X
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
