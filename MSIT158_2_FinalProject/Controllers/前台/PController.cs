using Microsoft.AspNetCore.Mvc;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.Models.前台;
using System.Security.Cryptography;
using System.Text.Json;

namespace MSIT158_2_FinalProject.Controllers.前台
{
	public class PController : Microsoft.AspNetCore.Mvc.Controller
	{
		public IActionResult Productpage(int id = 15)
		{
			SelectShopContext db = new SelectShopContext();
            // 根據商品ID查詢商品資訊
            TProduct Product = db.TProducts.FirstOrDefault(x => x.ProductId == id);
			ViewBag.Product = Product; // 將商品資訊放入ViewBag
            // 查詢該商品的全部評價並聯接會員資訊
            var 此商品全部評價 = db.TReviews.Where(x => x.ProductId == id).Join(db.TMembers, x => x.MemberId, y => y.MemberId, (x, y) =>
			new { x.ReviewDate, x.RankId, x.Comment, y.MemberName}); // 將評價資訊放入ViewBag
            ViewBag.此商品全部評價 = 此商品全部評價;
            // 查詢商品的類別資訊並聯接主類別資訊
            var 商品類別 = db.TSubCategories.Where(x => x.SubCategoryId == Product.SubCategoryId).Join(db.TCategories, x => x.CategoryId, y => y.CategoryId, (x, y) =>
			new { x.SubCategoryCname, y.CategoryCname, x.SubCategoryId }).FirstOrDefault();
			ViewBag.商品類別 = 商品類別;
            // 查詢相關商品，排除當前商品並且狀態為1的商品
            IEnumerable<TProduct> 相關商品 = db.TProducts.Where(x => x.SubCategoryId == 商品類別.SubCategoryId).Where(x => x.ProductId != id&&x.Status==1);
			ViewBag.相關商品 = 相關商品.Take(4); // 將最多4個相關商品放入ViewBag
            // 計算商品的平均評分
            ViewBag.平均星 = Convert.ToInt32(此商品全部評價.Average(x => x.RankId));
            // 計算評價數量
            ViewBag.評價數 = 此商品全部評價.Count();
            // 將商品描述轉換為HTML格式換行符號
            ViewBag.商品詳細=new fM通用().改成HTML換行符號(Product.Description);
            // 計算各星級的百分比
            ViewBag.s5 = new fM通用().商品評價百分比(id, 5);
			ViewBag.s4 = new fM通用().商品評價百分比(id, 4);
			ViewBag.s3 = new fM通用().商品評價百分比(id, 3);
			ViewBag.s2 = new fM通用().商品評價百分比(id, 2);
			ViewBag.s1 = new fM通用().商品評價百分比(id, 1);
            // 計算各星級的評價數量
            ViewBag.d5 = 此商品全部評價.Where(x => x.RankId == 5).Count();
			ViewBag.d4 = 此商品全部評價.Where(x => x.RankId == 4).Count();
			ViewBag.d3 = 此商品全部評價.Where(x => x.RankId == 3).Count();
			ViewBag.d2 = 此商品全部評價.Where(x => x.RankId == 2).Count();
			ViewBag.d1 = 此商品全部評價.Where(x => x.RankId == 1).Count();
            // 查詢商品的品牌資訊
            ViewBag.品牌= db.TLabels.FirstOrDefault(x => x.LabelId == Product.LabelId);
            //int mid = 0;
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            //{
            //    string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
            //    TMember m = JsonSerializer.Deserialize<TMember>(js);
            //    mid = m.MemberId;
            //}
            //ViewBag.mid = mid;
            return View();
		}
        public IActionResult ReviewsAPI([FromBody] DTO商品頁評價 a)
		{
			SelectShopContext db = new SelectShopContext();
            // 查詢特定商品的所有評價，並聯接會員資訊
            var 此商品全部評價 = db.TReviews.Where(x => x.ProductId == a.pid).Join(db.TMembers, x => x.MemberId, y => y.MemberId, (x, y) =>
			new { x.ReviewDate, x.RankId, x.Comment, y.MemberName, y.MemberPhoto,x.ReviewId} )
                .OrderByDescending(x=>x.ReviewDate) // 依評價日期降序排列
                .OrderByDescending(x => x.ReviewId) // 依評價ID降序排列
                .Skip((a.page - 1) * 7) // 跳過前面的頁數
                .Take(7); // 取出當前頁的評價
            return Json(此商品全部評價);
		}
        public IActionResult addCartAPI([FromBody] TCart a)
        {
            int mid = 0;
            // 檢查Session中是否包含登入會員的資訊
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            SelectShopContext db = new SelectShopContext();
            // 將商品添加到購物車
            new fM購物車().add購物車2(a);
            return Content("ok", "text/plain");
        }
        public IActionResult addPackageCartAPI(int id,int pid,int qty=1)
        {
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            //{
            //    string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
            //    TMember m = JsonSerializer.Deserialize<TMember>(js);
            //    mid = m.MemberId;
            //}
            //else return Content("沒登入", "text/plain");
            SelectShopContext db = new SelectShopContext();
            // 建立新的包裝購物車物件
            TPackageCart p = new TPackageCart();
            p.MemberId = id; // 設定會員ID
            p.PackageId = pid; // 設定包裝ID
            p.Qty = qty; // 設定數量，默認為1
            // 使用fM購物車類別中的add包裝方法將包裝加入到購物車
            new fM購物車().add包裝(p);
            return Content("ok", "text/plain");
        }
        /*
         * async function addPackageCart(id,qty) {
            var a2 = await fetch(`https://localhost:7066/p/addPackageCartAPI/2/?pid=11`)
            var b2 = await a2.text()
            if (b2 == "ok")alert("加入購物車成功")}
         */
    }
}
	
