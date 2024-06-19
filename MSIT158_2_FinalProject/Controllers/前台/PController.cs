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
			TProduct Product = db.TProducts.FirstOrDefault(x => x.ProductId == id);
			ViewBag.Product = Product;
			var 此商品全部評價 = db.TReviews.Where(x => x.ProductId == id).Join(db.TMembers, x => x.MemberId, y => y.MemberId, (x, y) =>
			new { x.ReviewDate, x.RankId, x.Comment, y.MemberName});
			ViewBag.此商品全部評價 = 此商品全部評價;
			var 商品類別 = db.TSubCategories.Where(x => x.SubCategoryId == Product.SubCategoryId).Join(db.TCategories, x => x.CategoryId, y => y.CategoryId, (x, y) =>
			new { x.SubCategoryCname, y.CategoryCname, x.SubCategoryId }).FirstOrDefault();
			ViewBag.商品類別 = 商品類別;
			IEnumerable<TProduct> 相關商品 = db.TProducts.Where(x => x.SubCategoryId == 商品類別.SubCategoryId).Where(x => x.ProductId != id&&x.Status==1);
			ViewBag.相關商品 = 相關商品.Take(4);
			ViewBag.平均星 = Convert.ToInt32(此商品全部評價.Average(x => x.RankId));
			ViewBag.評價數 = 此商品全部評價.Count();
			ViewBag.商品詳細=new fM通用().改成HTML換行符號(Product.Description);
			ViewBag.s5 = new fM通用().商品評價百分比(id, 5);
			ViewBag.s4 = new fM通用().商品評價百分比(id, 4);
			ViewBag.s3 = new fM通用().商品評價百分比(id, 3);
			ViewBag.s2 = new fM通用().商品評價百分比(id, 2);
			ViewBag.s1 = new fM通用().商品評價百分比(id, 1);
			ViewBag.d5 = 此商品全部評價.Where(x => x.RankId == 5).Count();
			ViewBag.d4 = 此商品全部評價.Where(x => x.RankId == 4).Count();
			ViewBag.d3 = 此商品全部評價.Where(x => x.RankId == 3).Count();
			ViewBag.d2 = 此商品全部評價.Where(x => x.RankId == 2).Count();
			ViewBag.d1 = 此商品全部評價.Where(x => x.RankId == 1).Count();
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
			var 此商品全部評價 = db.TReviews.Where(x => x.ProductId == a.pid).Join(db.TMembers, x => x.MemberId, y => y.MemberId, (x, y) =>
			new { x.ReviewDate, x.RankId, x.Comment, y.MemberName, y.MemberPhoto} ).OrderByDescending(x=>x.ReviewDate).Skip((a.page - 1) * 7).Take(7);
			return Json(此商品全部評價);
		}
        public IActionResult addCartAPI([FromBody] TCart a)
        {
            int mid = 0;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_MEMBER))
            {
                string js = HttpContext.Session.GetString(CDictionary.SK_LOGIN_MEMBER);
                TMember m = JsonSerializer.Deserialize<TMember>(js);
                mid = m.MemberId;
            }
            SelectShopContext db = new SelectShopContext();
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
            TPackageCart p = new TPackageCart();
            p.MemberId = id;
            p.PackageId = pid;
            p.Qty = qty;
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
	
