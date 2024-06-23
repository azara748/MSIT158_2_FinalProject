﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.Models.DTO;
using System.Security.Cryptography;

namespace MSIT158_2_FinalProject.Controllers.Front
{
	public class HomePageController : Controller
	{
		public IActionResult Search()
		{
			return View();
		}
		public IActionResult index(int? id)
		{
			if (id == null||id<=0) { return RedirectToAction("Search"); }
			ViewBag.Id = id;
			return View();
		}
        public async Task <IActionResult> RealHome()
        {
			SelectShopContext db = new SelectShopContext();
			DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
			var query = db.TProducts
							   .Include(p => p.TKeywords)
							   .Where(p => p.TKeywords.Any(k => k.Festival.Contains("父親節")))
							   .AsQueryable();

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
				isnew = pro.LaunchTime.HasValue && EF.Functions.DateDiffDay(pro.LaunchTime.Value, currentDate) < 30
			})
			.ToListAsync();

			return View(fatherProducts);
		}
			
        

    }
}
