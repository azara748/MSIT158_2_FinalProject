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
            // 檢查 Session 中是否包含登入會員的資料
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                // 從 Session 中取得登入會員的 JSON 字串
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                // 將 JSON 字串反序列化為 TMember 物件
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                // 取得會員 ID
                mid = m.MemberId;
            }
            else return RedirectToAction("Login", "HOME"); // 如果未登入，重新導向到 Login 動作方法
            SelectShopContext db = new SelectShopContext();
            // 查詢會員的購物車中的產品，查詢會員購物車 (TCarts 表) 中的產品資料，並與 TProducts 表進行連接，取得產品的詳細資訊。
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.ProductPhoto, y.ProductName, y.UnitPrice, y.ProductId, y.Stocks, x.Qty, x.CartId });
            ViewBag.會員購物車 = 會員購物車;
            // 設定運費
            ViewBag.運費 = 60;
            // 查詢會員的購物車中的包裹，查詢會員購物車 (TPackageCarts 表) 中的包裹資料，並與 TAllPackages 表進行連接，取得包裹的詳細資訊。
            var 會員購物車2 =db.TPackageCarts.Where(x => x.MemberId == mid).Join(db.TAllPackages, x => x.PackageId ,y => y.PackageId, (x, y) =>
            new { y.Picture, y.PackName, y.Price, x.Qty, x.PackageCartId });
            ViewBag.會員購物車2 = 會員購物車2;
            // 計算總價
            var 總價 = 會員購物車.Sum(x => x.UnitPrice * x.Qty)+ 會員購物車2.Sum(x => x.Price * x.Qty);
            ViewBag.總價 = 總價;
            // 返回視圖並將購物車資料傳遞給視圖
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
            // 查詢會員購物車 (TCarts 表) 中的產品資料，並與 TProducts 表進行連接，取得產品的詳細資訊。
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.ProductPhoto, y.ProductName, y.UnitPrice, y.ProductId, y.Stocks, x.Qty, x.CartId});
            // 返回 JSON 格式的購物車資料
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
            // 呼叫 fM購物車 類別中的 delete購物車 方法，刪除指定 id 的購物車項目
            new fM購物車().delete購物車(id);
            // 返回 "ok" 字串，表示操作成功
            return Content("ok", "text/plain");
        }
        // 調整購物車中指定商品的數量，用於增加或減少購物車中指定商品的數量。
        public IActionResult qtyCartAPI(int id, string q)
        {
            // 當 q 為 "p" 時，調用 plus購物車 方法增加數量；否則，調用 minus購物車 方法減少數量。
            if (q == "p") new fM購物車().plus購物車(id);
            else new fM購物車().minus購物車(id);
            return Content("ok", "text/plain");
        }
        public IActionResult deleteCartAPI2(int id)
        {
            // 刪除購物車中指定的包裝。
            new fM購物車().delete購物車2(id);
            return Content("ok", "text/plain");
        }
        // 調整購物車中指定商品的數量，用於增加或減少購物車中指定商品的數量。
        public IActionResult qtyCartAPI2(int id, string q)
        {
            // 當 q 為 "p" 時，調用 plus購物車2 方法增加數量；否則，調用 minus購物車2 方法減少數量。
            if (q == "p") new fM購物車().plus購物車2(id);
            else new fM購物車().minus購物車2(id);
            return Content("ok", "text/plain");
        }
        // 這個方法主要是用來處理顯示會員的運輸頁面，包括會員資料、購物車內的商品和總價。
        public IActionResult Shippingpage()
        {
            int mid = 0;
            // 檢查是否有登入會員
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            else return RedirectToAction("Login", "HOME");
            SelectShopContext db = new SelectShopContext();
            // 取得會員資料
            var 會員資料 = db.TMembers.FirstOrDefault(x => x.MemberId == mid);
            // 取得會員購物車中的商品
            var 會員購物車 = db.TCarts.Where(x => x.MemberId == mid).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) => 
            new {  y.ProductName, y.UnitPrice,  x.Qty});
            // 取得會員購物車中的包裝
            var 會員購物車2 = db.TPackageCarts.Where(x => x.MemberId == mid).Join(db.TAllPackages, x => x.PackageId, y => y.PackageId, (x, y) =>
            new { y.PackName, y.Price, x.Qty});
            // 設定運費
            ViewBag.運費 = 60;
            ViewBag.會員購物車 = 會員購物車;
            ViewBag.會員資料 = 會員資料;         
            ViewBag.會員購物車2 = 會員購物車2;
            // 計算總價
            ViewBag.總價 =  Convert.ToInt32(會員購物車.Sum(x => x.UnitPrice * x.Qty) + 會員購物車2.Sum(x => x.Price * x.Qty));
            return View();
        }
        [HttpPost]
        public IActionResult Shippingpage(TOrder value)
        {
            // 初始化會員ID
            int mid = 0;
            // 檢查是否有登入會員
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            // 設置訂單相關信息
            value.MemberId = mid;
            value.ShippingMethodId = 1;
            value.OrderDate = DateTime.Now;
            value.Reviewed=false;
            SelectShopContext db = new SelectShopContext();
            // 新增訂單到資料庫
            db.TOrders.Add(value);
            db.SaveChanges();

            // 獲取最新的訂單ID
            int lo = db.TOrders.OrderByDescending(x => x.OrderId).FirstOrDefault().OrderId;
            // 獲取會員購物車中的商品
            var a = db.TCarts.Where(x => x.MemberId == value.MemberId);
            // 將購物車商品添加到訂單明細
            foreach (var x in a)
            {
                TPurchase p = new TPurchase();
                p.OrderId = lo;
                p.ProductId = x.ProductId;
                p.Qty = x.Qty;
                db.TPurchases.Add(p);
            }
            // 刪除會員購物車中的商品
            db.TCarts.Where(x => x.MemberId == value.MemberId).ExecuteDelete();
            // 獲取會員購物車中的包裝
            var b = db.TPackageCarts.Where(x => x.MemberId == value.MemberId);
            // 將包裝商品添加到訂單包裝明細
            foreach (var x in b)
            {
                TPackageWayDetail p = new TPackageWayDetail();
                p.OrderId = lo;
                p.PackageId = x.PackageId;
                p.PackQty = x.Qty;
                db.TPackageWayDetails.Add(p);
            }
            // 刪除會員購物車中的包裝商品
            db.TPackageCarts.Where(x => x.MemberId == value.MemberId).ExecuteDelete();
            // 保存所有更改   
            db.SaveChanges();
            // 根據訂單狀態重定向，根據訂單狀態重定向到不同的頁面。
            if (value.StatusId==2) return RedirectToAction("Memberpage", "M"); 
            else   return RedirectToAction("CashFlow", "HOME", new { totalAmount =value.CheckoutAmount,oid=lo });
        }
        //[HttpPost]
        //public IActionResult Shippingpage2(int id)
        //{           
        //    SelectShopContext db = new SelectShopContext();
        //    TOrder o = db.TOrders.FirstOrDefault(x => x.OrderId==id);
        //     o.StatusId = 2;
        //     o.OrderDate = DateTime.Now;
        //    db.SaveChanges();
        //    return RedirectToAction("Productpage", "P");
        //}
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
