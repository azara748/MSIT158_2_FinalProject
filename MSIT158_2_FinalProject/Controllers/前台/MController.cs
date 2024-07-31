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
            // 初始化會員ID
            int mid = 2;
            // 檢查是否有登入會員
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
             else return RedirectToAction("Login", "HOME");

            SelectShopContext db = new SelectShopContext();
            // 獲取會員信息
            ViewBag.會員 = db.TMembers.FirstOrDefault(x=>x.MemberId==mid);
            // 獲取訂單商品明細
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
           Freight = o.Freight,
           Photo = pro.ProductPhoto,
           Photo2 = "",
           Name = pro.ProductName,
       };
            // 獲取訂單包裝明細
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
              Freight = o.Freight,
              Photo = (byte[])null,
              Photo2 = apk.Picture,
              Name = apk.PackName,
          };
            // 合併訂單商品和包裝明細，並按訂單分組
            var v = v1.Union(v2)
           .GroupBy(x => new { x.OrderId, x.OrderDate, x.StatusId, x.Reviewed,x.Freight})
           .Select(g => new
           {
               g.Key.OrderId,
               g.Key.OrderDate,
               g.Key.StatusId,
               g.Key.Reviewed,
               Freight=g.Key.Freight==null? 0 : g.Key.Freight,
               總數 = g.Sum(x => x.Qty),
               總價 = g.Sum(x => x.Qty * x.UnitPrice)+g.Key.Freight,
               name = g.FirstOrDefault().Name,
               圖片 = g.FirstOrDefault().Photo,
               圖片2 = g.FirstOrDefault().Photo2,
           }).OrderByDescending(x => x.OrderDate).ToList();

            // 根據訂單狀態篩選
            if (id == 1) v = v.Where(x => x.StatusId == 1).ToList();
            else if (id == 2) v = v.Where(x => x.StatusId == 2).ToList();
            else if (id == 3) v = v.Where(x => x.StatusId == 3).ToList();

            // 設置每頁顯示數量
            int 顯示數 = 15;

            // 計算總頁數
            int allpage = 0;
            if (v.Count() < 顯示數+1) allpage = 1;
            else if (v.Count() % 顯示數 == 0) allpage = v.Count() / 顯示數;
            else allpage = v.Count() / 顯示數 + 1;
            // 確保頁數不超出範圍
            if (page > allpage) page = allpage;
            ViewBag.allpage = allpage;
            ViewBag.page = page;
            ViewBag.id = id;

            if (id != 0) v = v.Where(x => x.StatusId == id).ToList();
            // 根據頁數篩選顯示的訂單  
            ViewBag.訂單預覽 = v.Skip((page - 1) * 顯示數).Take(顯示數);

            return View();
        }
        public IActionResult upMemberimgAPI(IFormFile IMG)
        {
            // 檢查是否有上傳的圖片
            if (IMG != null)
            {
                int mid = 2; // 預設會員ID為2
                // 檢查Session中是否包含登入會員的資訊
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
                {
                    // 從Session中取得會員資訊並反序列化成TMember物件
                    string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                    TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                    mid = m.MemberId;
                }
                SelectShopContext db = new SelectShopContext();
                // 根據會員ID查詢會員資料
                TMember a = db.TMembers.FirstOrDefault(x => x.MemberId == mid);
                // 生成新的圖片檔名 
                string name = Guid.NewGuid().ToString() + ".jpg";
                // 將上傳的圖片儲存到伺服器上的指定路徑
                IMG.CopyTo(new FileStream(_img.WebRootPath + "/img/" + name, FileMode.Create));
                a.MemberPhoto = name; // 更新會員的照片屬性
                db.SaveChanges(); // 儲存資料庫變更
                return Content("ok", "text/plain"); // 返回成功訊息
            }
            else return Content("no", "text/plain"); // 沒有上傳圖片時返回失敗訊息
        }
        public IActionResult Purchasepage(int id)
        {
            int mid = 2; // 預設會員ID為2
            // 檢查Session中是否包含登入會員的資訊
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                // 從Session中取得會員資訊並反序列化成TMember物件   
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId; // 取得會員ID
            }
            else return RedirectToAction("Login", "HOME"); // 若未登入則跳轉到登入頁面

            SelectShopContext db = new SelectShopContext();
            // 查詢購買訂單與商品資訊，並將結果放入ViewBag
            var a = db.TPurchases.Where(x => x.OrderId == id).Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.UnitPrice, y.ProductName, y.ProductPhoto, x.Qty, y.ProductId });
            ViewBag.a = a;
            // 查詢包裹方式與包裹資訊，並將結果放入ViewBag
            var b = db.TPackageWayDetails.Where(x => x.OrderId == id).Join(db.TAllPackages, x => x.PackageId, y => y.PackageId, (x, y) =>
            new { y.Price, y.PackName, y.Picture, x.PackQty });
            ViewBag.b = b;

            // 查詢訂單與購買的商品詳細資訊
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
          Name = pro.ProductName,
          Freight = o.Freight,
          pay=o.PaymentType
      };
            // 查詢訂單與包裹的詳細資訊
            var v2 =
          from o in db.TOrders
          join pkw in db.TPackageWayDetails
        on o.OrderId equals pkw.OrderId
          join apk in db.TAllPackages
        on pkw.PackageId equals apk.PackageId
          where o.OrderId==id
          select new
          {
              OrderId = o.OrderId,
              OrderDate = o.OrderDate,
              StatusId = o.StatusId,
              Reviewed = o.Reviewed,
              Qty = pkw.PackQty,
              UnitPrice = apk.Price,
              Name = apk.PackName,
              Freight = o.Freight,
              pay = o.PaymentType
          };

            // 合併商品與包裹的訂單資訊，並進行分組計算
            var v = v1.Union(v2)
           .GroupBy(x => new { x.OrderId, x.OrderDate, x.StatusId, x.Reviewed,x.Freight,x.pay })
           .Select(g => new
           {
               g.Key.OrderId,
               g.Key.OrderDate,
               g.Key.StatusId,
               g.Key.Reviewed,
               Freight=g.Key.Freight==null?0: g.Key.Freight,
               pay=g.Key.pay == null ? "貨到付款" : g.Key.pay,
               總數 = g.Sum(x => x.Qty),
               總價 = g.Sum(x => x.Qty * x.UnitPrice)+g.Key.Freight,
               name = g.FirstOrDefault().Name,
           });
            ViewBag.o = v.FirstOrDefault(); // 將合併的結果放入ViewBag
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
            // 查詢購買訂單與商品資訊
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
            ViewBag.v = v; // 將查詢結果放入ViewBag
            return View();
        }
        public IActionResult ReviewecheckAPI([FromForm] TReview id)
        {         
            SelectShopContext db = new SelectShopContext();
            // 查詢敏感詞彙，條件為WordSeverityId等於1
            var a = db.TWordCensorships.Where(x=>x.WordSeverityId==1);
            foreach(var v in a)
            {
                // 如果評論為空，則跳出迴圈
                if (string.IsNullOrEmpty(id.Comment))break;
                // 檢查評論中是否包含敏感詞彙（不區分大小寫）
                if (id.Comment.ToLower().Contains(v.Word.ToLower()))
                {
                    return Content("no", "text/plain"); // 如果包含敏感詞彙，返回"no"
                }
            }
            return Content("ok", "text/plain"); // 如果沒有敏感詞彙，返回"ok"
        }
        public IActionResult upRevieweAPI([FromForm] TReview id,int oid)
        {
            int mid = 0;
            // 檢查Session中是否包含登入會員的資訊
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = System.Text.Json.JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            SelectShopContext db = new SelectShopContext();
            // 設定評論日期為當前日期，格式為"yyyy/MM/dd"
            id.ReviewDate=DateTime.Now.ToString("yyyy/MM/dd");
            id.MemberId = mid; // 設定會員ID
            db.TReviews.Add(id); // 將評論加入到資料庫中
            // 設定訂單的Reviewed屬性為true
            var o = db.TOrders.FirstOrDefault(x => x.OrderId == oid).Reviewed = true;
            db.SaveChanges(); // 儲存資料庫變更
            return Content("ok", "text/plain");
        }
    }
}

