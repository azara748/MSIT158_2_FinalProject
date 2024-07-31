using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MSIT158_2_FinalProject.Models;
using System.Security.Cryptography;

namespace MSIT158_2_FinalProject.Controllers.後台
{
    public class WController : Controller
    {
        public IActionResult WordCensorship(int page = 1, string keyword = "", string x = "", int ode = 0, string delect = "",int boo=0)
        {
            SelectShopContext db = new SelectShopContext();
            // 如果需要刪除評論
            if (delect == "delect")
            {
                // 刪除包含特定關鍵字的評論
                if (x != "ProductName")
                    db.TReviews.Where(x => x.Comment.ToLower().Contains(keyword.ToLower())).ExecuteDelete();
                else
                {
                    // 刪除包含特定商品名稱的評論
                    var xx = db.TProducts.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower())).ToList();
                    foreach(var xxx in xx)
                    db.TReviews.Where(x => x.ProductId == xxx.ProductId).ExecuteDelete();
                }
                db.SaveChanges(); // 保存變更
            }


            // 查詢評論及其相關的產品信息
            var v = db.TReviews.Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.ProductPhoto, y.ProductName, x.ReviewDate, x.RankId, x.Comment, x.ReviewId,x.ProductId});

            ViewBag.x = "";

            // 如果有關鍵字篩選 
            if (!string.IsNullOrEmpty(keyword))
            {
                if (x == "ProductName")
                {
                    //var name = db.TProducts.FirstOrDefault(x=>x.ProductId==v.)
                    v = v.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower()));
                    ViewBag.x = "";
                }
                else
                {
                    v = v.Where(x => x.Comment.ToLower().Contains(keyword.ToLower()));
                    ViewBag.x = "Comment";
                }
            }
            ViewBag.keyword = keyword;
            ViewBag.boo = 0;
            // 篩選不為空的評論 
            if (boo == 1)
            {
                v = v.Where(x => !string.IsNullOrEmpty(x.Comment));
                ViewBag.boo = 1;
            }

            // 根據選項排序評論 
            if (ode == 0) v = v.OrderByDescending(x => x.ReviewId);
            else if (ode == 1) v = v.OrderByDescending(x => x.RankId);
            else if (ode == 2) v = v.OrderBy(x => x.RankId);
            else if (ode == 3) v = v.OrderByDescending(x => x.ReviewId).OrderByDescending(x => x.ReviewDate);
            else if (ode == 4) v = v.OrderBy(x => x.ReviewId).OrderBy(x => x.ReviewDate);
            ViewBag.ode = ode;

            int 顯示數 = 25;

            // 計算總頁數
            int allpage = 0;
            if (v.Count() < 顯示數 + 1) allpage = 1;
            else if (v.Count() % 顯示數 == 0) allpage = v.Count() / 顯示數;
            else allpage = v.Count() / 顯示數 + 1;
            // 確保頁數在合法範圍內
            if (page > allpage) page = allpage;
            if (page < 1) page = 1;
            ViewBag.allpage = allpage;
            ViewBag.page = page;
            ViewBag.評論 = v.Skip((page - 1) * 顯示數).Take(顯示數); ;

            return View();
        }
        public IActionResult delectReviewAPI(int id)
        {
            SelectShopContext db = new SelectShopContext();
            int v = db.TReviews.Where(x => x.ReviewId == id).ExecuteDelete(); // 刪除指定評論
            return Content("ok", "text/plain");
        }
        public IActionResult WordCensorship2(string keyword = "")
        {
            SelectShopContext db = new SelectShopContext();
            // 查詢所有敏感詞並按ID降序排列
            IEnumerable<TWordCensorship> a = db.TWordCensorships.OrderByDescending(x=>x.WordCensorshipId);
            // 根據關鍵字篩選敏感詞
            if (!string.IsNullOrEmpty(keyword))
                a =a.Where(x => x.Word.ToLower().Contains(keyword.ToLower()));
            return View(a); // 返回篩選結果視圖
        }
        public IActionResult addWordCensorship2(string word)
        {
            SelectShopContext db = new SelectShopContext();
            TWordCensorship wordCensorship = new TWordCensorship(); // 創建新的敏感詞對象
            wordCensorship.Word = word; // 設定敏感詞
            wordCensorship.WordSeverityId = 1; // 設定敏感詞嚴重程度
            db.TWordCensorships.Add(wordCensorship); // 將敏感詞加入資料庫
            db.SaveChanges(); // 保存變更
            return RedirectToAction("WordCensorship2"); // 重定向到敏感詞管理頁面
        }
        public IActionResult delectWordCensorshipAPI(int id)
        {
            SelectShopContext db = new SelectShopContext();
            // 刪除指定敏感詞
            int v = db.TWordCensorships.Where(x => x.WordCensorshipId == id).ExecuteDelete();
            return Content("ok", "text/plain");
        }
    }
}
