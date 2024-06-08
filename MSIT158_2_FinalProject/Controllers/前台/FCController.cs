using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MSIT158_2_FinalProject.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace MSIT158_2_FinalProject.Controllers.前台
{
    public class FCController : Controller
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    base.OnActionExecuting(context);
        //    if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //        {
        //            Controller = "Home",
        //            action = "Login"
        //        }));
        //    }
        //}
        public IActionResult Index()
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            if (mid == 0) return RedirectToAction("Login", "HOME");
            SelectShopContext db = new SelectShopContext();
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.ProductPhoto, y.ProductName, y.UnitPrice, y.Stocks, x.Qty, x.CartId, x.Checking });
            ViewBag.會員購物車 = 會員購物車;
            var 總價 = 會員購物車.Sum(x => x.UnitPrice * x.Qty);
            ViewBag.總價 = 總價;
            ViewBag.運費 = 60;
            return View(會員購物車);
        }
        public IActionResult Index2()
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            SelectShopContext db = new SelectShopContext();
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.ProductPhoto, y.ProductName, y.UnitPrice, y.Stocks, x.Qty, x.CartId, x.Checking });
            return Json(會員購物車);
        }
        public IActionResult Index3(int id)
        {
            new fM購物車().delete購物車(id);
            return Content("ok", "text/plain");
        }
        public IActionResult Index4(int id, string q)
        {
            if (q == "p") new fM購物車().plus購物車(id);
            else new fM購物車().minus購物車(id);
            return Content("ok", "text/plain");
        }
        public IActionResult Index5()
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            if (mid == 0) return RedirectToAction("Login", "HOME");
            SelectShopContext db = new SelectShopContext();
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid);
            ViewBag.會員購物車 = 會員購物車;
            var 購物車詳細 = 會員購物車.Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) => new { y.ProductPhoto, y.ProductName, y.UnitPrice, y.Stocks, x.Qty, x.CartId, x.Checking });
            ViewBag.購物車詳細 = 購物車詳細;
            ViewBag.總價 = Convert.ToInt32(購物車詳細.Sum(x => x.Qty * x.UnitPrice));
            ViewBag.運費 = 60;
            var 會員資料 = db.TMembers.FirstOrDefault(x => x.MemberId == mid);
            ViewBag.會員資料 = 會員資料;
            return View();
        }
        [HttpPost]
        public IActionResult Index5(TOrder value)
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            value.MemberId = mid;
            SelectShopContext db = new SelectShopContext();
            db.TOrders.Add(value);
            db.SaveChanges();
            int lo = db.TOrders.OrderBy(x => x.OrderId).FirstOrDefault().OrderId;
            var a = db.TCarts.Where(x => x.MemberId == value.MemberId);
            foreach (var x in a)
            {
                TPurchase p = new TPurchase();
                p.OrderId = lo;
                p.ProductId = x.ProductId;
                p.Qty = x.Qty;
                db.TPurchases.Add(p);
                db.TCarts.Remove(x);
            }
            db.SaveChanges();
            return RedirectToAction("Index","FPD");
        }
        //public IActionResult Index6([FromBody]TOrder value)
        //{
        //    SelectShopContext db = new SelectShopContext();
        //    db.TOrders.Add(value);
        //    db.SaveChanges();
        //    int lo = db.TOrders.OrderBy(x => x.OrderId).FirstOrDefault().OrderId;
        //    var a = db.TCarts.Where(x => x.MemberId == value.MemberId);
        //    foreach (var x in a)
        //    {
        //        TPurchase p = new TPurchase();
        //        p.OrderId = lo;
        //        p.ProductId = x.ProductId;
        //        p.Qty = x.Qty;
        //        db.TPurchases.Add(p);
        //        db.TCarts.Remove(x);
        //    }
        //    db.SaveChanges();
        //    return null;
        //}
    }
}
