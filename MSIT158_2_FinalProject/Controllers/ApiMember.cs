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

        public IActionResult ClearSession()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGIN_MEMBER);
            return Ok("Session 已清除。");
        }
    }
}
