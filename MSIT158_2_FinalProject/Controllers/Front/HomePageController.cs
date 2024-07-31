using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.Models.DTO;
using System.Security.Cryptography;

namespace MSIT158_2_FinalProject.Controllers.Front
{
	public class HomePageController : Controller
	{
        // 搜尋動作方法
        //public IActionResult Search()
        public async Task<IActionResult> Search()
		{
            SelectShopContext db = new SelectShopContext();
            // 查詢 TSubCategories 表格中的資料
            var query = db.TSubCategories;
            // 將查詢結果投影到 subDTO 物件並轉為列表
            var subname = await query.Select(s => new subDTO
			{
				SubCategoryId = s.SubCategoryId,
				SubCategoryCname = s.SubCategoryCname
			}).ToListAsync();
            // 將結果傳遞到視圖
            return View(subname);
		}
        // 首頁動作方法
        public IActionResult index(int? id)
		{
            // 檢查 id 是否為 null 或小於等於 0
            if (id == null||id<=0) { return RedirectToAction("Search"); } // 重新導向到 Search 動作方法
            ViewBag.Id = id;
			return View();
		}
        // 真實首頁動作方法	
        public async Task <IActionResult> RealHome()
        {
			SelectShopContext db = new SelectShopContext();
            // 取得當前日期
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            // 查詢 TProducts 表格中的資料，並包含 TKeywords 關聯資料，且條件是 Festival 包含 "父親節"。
            var query = db.TProducts
							   .Include(p => p.TKeywords)
							   .Where(p => p.TKeywords.Any(k => k.Festival.Contains("父親節")))
							   .AsQueryable();

            // 將查詢結果投影到 faDTO 物件並轉為列表，並計算各種屬性如 Score 和 isnew。
            var fatherProducts = await query.Select(pro => new faDTO
			{
				ProductId = pro.ProductId,
				ProductName = pro.ProductName,
				Productphoto = pro.ProductPhoto,
				SubCategoryId = pro.SubCategoryId,
				SubCatName = pro.SubCategory.SubCategoryCname,
				Stocks = pro.Stocks,
				UnitPrice = pro.UnitPrice,
				Discount = pro.Active.Discount,
				LabelName = pro.Label.LabelName,
				Score = pro.TReviews.Average(p => p.RankId),
				isnew = pro.LaunchTime.HasValue && EF.Functions.DateDiffDay(pro.LaunchTime.Value, currentDate) < 60
			})
			.ToListAsync();

			return View(fatherProducts);
		}
			
        

    }
}
