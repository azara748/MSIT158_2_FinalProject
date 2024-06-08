using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;
using System.Text;

namespace MSIT158_2_FinalProject.Controllers.前台
{
    public class FMBController : Controller
    {
        public IWebHostEnvironment _img;
        public FMBController(IWebHostEnvironment img)
        {
            _img = img;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index1(IFormFile IMG)
        {
            if (IMG != null)
            {
                int mid = 2;
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
                {
                    string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                    TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                    mid = m.MemberId;
                }                
                SelectShopContext db = new SelectShopContext();
                TMember a =db.TMembers.FirstOrDefault(x => x.MemberId == mid);
                string name = Guid.NewGuid().ToString() + ".jpg";
                IMG.CopyTo(new FileStream(_img.WebRootPath + "/img/" + name, FileMode.Create));              
                a.MemberPhoto = name;
                return Content("ok", "text/plain");
            }
            else return Content("no", "text/plain");
        }
    }
}
