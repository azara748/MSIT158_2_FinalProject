using Blazor_MSIT158_2.Models.前台;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using MSIT158_2_FinalProject.Models;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MSIT158_2_FinalProject.Controllers.前台
{
    public class MController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IWebHostEnvironment _img;
        public MController(IWebHostEnvironment img)
        {
            _img = img;
        }
        public IActionResult Memberpage(int id = 0, int page = 1)
        {
            int mid = 2;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
             else return RedirectToAction("Login", "HOME");

            SelectShopContext db = new SelectShopContext();
            ViewBag.會員 = db.TMembers.FirstOrDefault(x=>x.MemberId==mid);
           var v1 =
       from o in db.TOrders
       join p in db.TPurchases
       on o.OrderId equals p.OrderId
       join pro in db.TProducts
       on p.ProductId equals pro.ProductId
       where o.MemberId == mid
       select new
       {
           OrderId = o.OrderId,
           OrderDate = o.OrderDate,
           StatusId = o.StatusId,
           Reviewed = o.Reviewed,
           Qty = p.Qty,
           UnitPrice = pro.UnitPrice,
           Photo = pro.ProductPhoto,
           Photo2 = "",
           Name = pro.ProductName,
       };
            var v2 =
          from o in db.TOrders
          join pkw in db.TPackageWayDetails
        on o.OrderId equals pkw.OrderId
          join apk in db.TAllPackages
        on pkw.PackageId equals apk.PackageId
          where o.MemberId == mid
          select new
          {
              OrderId = o.OrderId,
              OrderDate = o.OrderDate,
              StatusId = o.StatusId,
              Reviewed = o.Reviewed,
              Qty = pkw.PackQty,
              UnitPrice = apk.Price,
              Photo = (byte[])null,
              Photo2 = apk.Picture,
              Name = apk.PackName,
          };

            var v = v1.Union(v2)
           .GroupBy(x => new { x.OrderId, x.OrderDate, x.StatusId, x.Reviewed })
           .Select(g => new
           {
               g.Key.OrderId,
               g.Key.OrderDate,
               g.Key.StatusId,
               g.Key.Reviewed,
               總數 = g.Sum(x => x.Qty),
               總價 = g.Sum(x => x.Qty * x.UnitPrice),
               name = g.FirstOrDefault().Name,
               圖片 = g.FirstOrDefault().Photo,
               圖片2 = g.FirstOrDefault().Photo2,
           }).OrderByDescending(x => x.OrderDate).ToList();




            if (id != 0) v = v.Where(x => x.StatusId == id).ToList();
            ViewBag.訂單預覽 = v.Skip((page - 1) * 10).Take(page * 10);

            int allpage = 0;
            if (v.Count() < 11) allpage = 1;
            else if (v.Count() % 10 == 0) allpage = v.Count() / 10;
            else allpage = v.Count() / 10 + 1;
            if (page > allpage) page = allpage;
            ViewBag.allpage = allpage;
            ViewBag.page = page;
            ViewBag.id = id;


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
                TMember a = db.TMembers.FirstOrDefault(x => x.MemberId == mid);
                string name = Guid.NewGuid().ToString() + ".jpg";
                IMG.CopyTo(new FileStream(_img.WebRootPath + "/img/" + name, FileMode.Create));
                a.MemberPhoto = name;
                db.SaveChanges();
                return Content("ok", "text/plain");
            }
            else return Content("no", "text/plain");
        }
        public IActionResult Purchasepage(int id)
        {
            int mid = 2;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            else return RedirectToAction("Login", "HOME");

            SelectShopContext db = new SelectShopContext();
            var a = db.TPurchases.Where(x => x.OrderId == id).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.UnitPrice, y.ProductName, y.ProductPhoto, x.Qty, y.ProductId });
            ViewBag.a = a;
            var b = db.TPackageWayDetails.Where(x => x.OrderId == id).Join(db.TAllPackages, x => x.PackageId, y => y.PackageId, (x, y) =>
            new { y.Price, y.PackName, y.Picture, x.PackQty });
            ViewBag.b = b;

            var v1 =
        from o in db.TOrders
        join p in db.TPurchases
        on o.OrderId equals p.OrderId
        join pro in db.TProducts
        on p.ProductId equals pro.ProductId
        where o.OrderId == id
        select new
        {
            OrderId = o.OrderId,
            OrderDate = o.OrderDate,
            StatusId = o.StatusId,
            Reviewed = o.Reviewed,
            Qty = p.Qty,
            UnitPrice = pro.UnitPrice,
        };
            var v2 =
          from o in db.TOrders
          join pkw in db.TPackageWayDetails
        on o.OrderId equals pkw.OrderId
          join apk in db.TAllPackages
        on pkw.PackageId equals apk.PackageId
          where o.OrderId == id
          select new
          {
              OrderId = o.OrderId,
              OrderDate = o.OrderDate,
              StatusId = o.StatusId,
              Reviewed = o.Reviewed,
              Qty = pkw.PackQty,
              UnitPrice = apk.Price,
          };

            var v = v1.Union(v2)
           .GroupBy(x => new { x.OrderId, x.OrderDate, x.StatusId, x.Reviewed })
           .Select(g => new
           {
               g.Key.OrderId,
               g.Key.OrderDate,
               g.Key.StatusId,
               g.Key.Reviewed,
               總數 = g.Sum(x => x.Qty),
               總價 = g.Sum(x => x.Qty * x.UnitPrice),
           }).ToList();

            ViewBag.o = v.FirstOrDefault();
            return View();
        }
        public IActionResult Reviewepage(int id)
        {
            int mid = 2;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            else return RedirectToAction("Login", "HOME");
            SelectShopContext db = new SelectShopContext();
            var v =
            from pu in db.TPurchases
            join pro in db.TProducts
            on pu.ProductId equals pro.ProductId
            where pu.OrderId == id
            select new
            {
                pu.OrderId,
                pu.PurchaseId,
                pro.ProductId,
                pro.ProductName,
                pro.ProductPhoto
            };
            ViewBag.v = v;
            return View();
        }
        public IActionResult ReviewecheckAPI([FromForm] TReview id)
        {         
            SelectShopContext db = new SelectShopContext();
            var a = db.TWordCensorships.Where(x=>x.WordSeverityId==1);
            foreach(var v in a)
            {
                if(string.IsNullOrEmpty(id.Comment))break;
                if (id.Comment.Contains(v.Word))
                {
                    return Content("no", "text/plain");
                }
            }
            return Content("ok", "text/plain");
        }
        public IActionResult upRevieweAPI([FromForm] TReview id,int oid)
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            SelectShopContext db = new SelectShopContext();
            id.ReviewDate=DateTime.Now.ToString("yyyy/MM/dd");
            id.MemberId = mid;
            db.TReviews.Add(id);
            var o = db.TOrders.FirstOrDefault(x => x.OrderId == oid).Reviewed = true;
            db.SaveChanges();
            return Content("ok", "text/plain");
        }
    }
}

