using Blazor_MSIT158_2.Models.前台;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using MSIT158_2_FinalProject.Models;
using System.Text;

namespace MSIT158_2_FinalProject.Controllers.前台
{
    public class MController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IWebHostEnvironment _img;
        public MController(IWebHostEnvironment img)
        {
            _img = img;
        }
        public IActionResult Memberpage()
        {
            int mid =2;
            SelectShopContext db = new SelectShopContext();
            var v =
    (from o in db.TOrders
     join p in db.TPurchases
     on o.OrderId equals p.OrderId into opJoin
     from p in opJoin.DefaultIfEmpty()
     join pro in db.TProducts
     on p.ProductId equals pro.ProductId into pproJoin
     from pro in pproJoin.DefaultIfEmpty()
     join pkw in db.TPackageWayDetails
     on o.OrderId equals pkw.OrderId into opkwJoin
     from pkw in opkwJoin.DefaultIfEmpty()
     join apk in db.TAllPackages
     on pkw.PackageId equals apk.PackageId into apkJoin
     from apk in apkJoin.DefaultIfEmpty()
     where o.MemberId == mid
     group new { o, pro, p, apk, pkw } by new { o.OrderId, o.OrderDate, o.StatusId } into A
     orderby A.Key.OrderDate descending
     select new
     {
         A.Key.OrderId,
         A.Key.OrderDate,
         A.Key.StatusId,
         總數 = A.Sum(x => (x.p != null ? x.p.Qty : 0) + (x.pkw != null ? x.pkw.PackQty : 0)),
         總價 = A.Sum(x => (x.pro != null ? x.pro.UnitPrice * (x.p != null ? x.p.Qty : 0) : 0) + (x.apk != null ? x.apk.Price * (x.pkw != null ? x.pkw.PackQty : 0) : 0)),
         圖片 = A.FirstOrDefault().pro.ProductPhoto,
         圖片2 = A.FirstOrDefault().apk.Picture,
         name  = A.FirstOrDefault().pro.ProductName != null ? A.FirstOrDefault().pro.ProductName : A.FirstOrDefault().apk.PackName,
     }).Take(10);

            ViewBag.訂單預覽 = v;
            return View();
        }
        public IActionResult upMemberimgAPI(IFormFile IMG)
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
