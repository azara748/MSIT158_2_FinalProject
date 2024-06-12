using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace MSIT158_2_FinalProject.Controllers.前台
{
    public class CController : Microsoft.AspNetCore.Mvc.Controller
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
        public IActionResult Cartspage()
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            else return RedirectToAction("Login", "HOME");
            SelectShopContext db = new SelectShopContext();
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.ProductPhoto, y.ProductName, y.UnitPrice, y.ProductId, y.Stocks, x.Qty, x.CartId });
            ViewBag.會員購物車 = 會員購物車;           
            ViewBag.運費 = 60;
            var 會員購物車2 =db.TPackageCarts.Where(x => x.MemberId == mid).Join(db.TAllPackages, x => x.PackageId ,y => y.PackageId, (x, y) =>
            new { y.Picture, y.PackName, y.Price, x.Qty, x.PackageCartId });
            ViewBag.會員購物車2 = 會員購物車2;
            var 總價 = 會員購物車.Sum(x => x.UnitPrice * x.Qty)+ 會員購物車2.Sum(x => x.Price * x.Qty);
            ViewBag.總價 = 總價;
            return View(會員購物車);
        }
        public IActionResult miniCartsAPI()
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
            new { y.ProductPhoto, y.ProductName, y.UnitPrice, y.ProductId, y.Stocks, x.Qty, x.CartId});
            return Json(會員購物車);
        }
        public IActionResult miniCartsAPI2()
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            SelectShopContext db = new SelectShopContext();
            var 會員購物車2 = db.TPackageCarts.Where(x => x.MemberId == mid).Join(db.TAllPackages, x => x.PackageId, y => y.PackageId, (x, y) =>
           new { y.Picture, y.PackName, y.Price, x.Qty, x.PackageCartId });
            return Json(會員購物車2);
        }
        public IActionResult deleteCartAPI(int id)
        {
            new fM購物車().delete購物車(id);
            return Content("ok", "text/plain");
        }
        public IActionResult qtyCartAPI(int id, string q)
        {
            if (q == "p") new fM購物車().plus購物車(id);
            else new fM購物車().minus購物車(id);
            return Content("ok", "text/plain");
        }
        public IActionResult deleteCartAPI2(int id)
        {
            new fM購物車().delete購物車2(id);
            return Content("ok", "text/plain");
        }
        public IActionResult qtyCartAPI2(int id, string q)
        {
            if (q == "p") new fM購物車().plus購物車2(id);
            else new fM購物車().minus購物車2(id);
            return Content("ok", "text/plain");
        }
        public IActionResult Shippingpage()
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            else return RedirectToAction("Login", "HOME");
            SelectShopContext db = new SelectShopContext();
            var 會員資料 = db.TMembers.FirstOrDefault(x => x.MemberId == mid);
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) => 
            new {  y.ProductName, y.UnitPrice,  x.Qty});
            var 會員購物車2 = db.TPackageCarts.Where(x => x.MemberId == mid).Join(db.TAllPackages, x => x.PackageId, y => y.PackageId, (x, y) =>
            new { y.PackName, y.Price, x.Qty});
            ViewBag.運費 = 60;
            ViewBag.會員購物車 = 會員購物車;
            ViewBag.會員資料 = 會員資料;         
            ViewBag.會員購物車2 = 會員購物車2;
            ViewBag.總價 =  Convert.ToInt32(會員購物車.Sum(x => x.UnitPrice * x.Qty) + 會員購物車2.Sum(x => x.Price * x.Qty));
            return View();
        }
        [HttpPost]
        public IActionResult Shippingpage(TOrder value)
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            value.MemberId = mid;
            value.ShippingMethodId = 1;
            value.OrderDate = DateTime.Now;
            value.Reviewed=false;
            SelectShopContext db = new SelectShopContext();
            db.TOrders.Add(value);
            db.SaveChanges();

            int lo = db.TOrders.OrderByDescending(x => x.OrderId).FirstOrDefault().OrderId;
            var a = db.TCarts.Where(x => x.MemberId == value.MemberId);
            foreach (var x in a)
            {
                TPurchase p = new TPurchase();
                p.OrderId = lo;
                p.ProductId = x.ProductId;
                p.Qty = x.Qty;
                db.TPurchases.Add(p);
            }
            db.TCarts.Where(x => x.MemberId == value.MemberId).ExecuteDelete();
            var b = db.TPackageCarts.Where(x => x.MemberId == value.MemberId);
            foreach (var x in b)
            {
                TPackageWayDetail p = new TPackageWayDetail();
                p.OrderId = lo;
                p.PackageId = x.PackageId;
                p.PackQty = x.Qty;
                db.TPackageWayDetails.Add(p);
            }
            db.TPackageCarts.Where(x => x.MemberId == value.MemberId).ExecuteDelete();
            db.SaveChanges();
            if(value.StatusId==2) return RedirectToAction("Memberpage", "M"); 
             else   return RedirectToAction("Productpage", "P");
        }
        [HttpPost]
        public IActionResult Shippingpage2(int id)
        {           
            SelectShopContext db = new SelectShopContext();
            TOrder o = db.TOrders.FirstOrDefault(x => x.OrderId==id);
             o.StatusId = 2;
             o.OrderDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Productpage", "P");
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
