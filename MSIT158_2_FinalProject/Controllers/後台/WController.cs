﻿using Microsoft.AspNetCore.Mvc;
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
            if (delect == "delect")
            {
                if (x != "ProductName")
                    db.TReviews.Where(x => x.Comment.ToLower().Contains(keyword.ToLower())).ExecuteDelete();
                else
                {
                    var xx = db.TProducts.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower())).ToList();
                    foreach(var xxx in xx)
                    db.TReviews.Where(x => x.ProductId == xxx.ProductId).ExecuteDelete();
                }
                db.SaveChanges();
            }
            


            var v = db.TReviews.Join(db.TProducts, x => x.ProductId, y => y.ProductId, (x, y) =>
            new { y.ProductPhoto, y.ProductName, x.ReviewDate, x.RankId, x.Comment, x.ReviewId,x.ProductId});

            ViewBag.x = "";

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
            if (boo == 1)
            {
                v = v.Where(x => !string.IsNullOrEmpty(x.Comment));
                ViewBag.boo = 1;
            }

            if (ode == 0) v = v.OrderByDescending(x => x.ReviewId);
            else if (ode == 1) v = v.OrderByDescending(x => x.RankId);
            else if (ode == 2) v = v.OrderBy(x => x.RankId);
            else if (ode == 3) v = v.OrderByDescending(x => x.ReviewId).OrderByDescending(x => x.ReviewDate);
            else if (ode == 4) v = v.OrderBy(x => x.ReviewId).OrderBy(x => x.ReviewDate);
            ViewBag.ode = ode;

            int 顯示數 = 25;

            int allpage = 0;
            if (v.Count() < 顯示數 + 1) allpage = 1;
            else if (v.Count() % 顯示數 == 0) allpage = v.Count() / 顯示數;
            else allpage = v.Count() / 顯示數 + 1;
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
            int v = db.TReviews.Where(x => x.ReviewId == id).ExecuteDelete();
            return Content("ok", "text/plain");
        }
        public IActionResult WordCensorship2(string keyword = "")
        {
            SelectShopContext db = new SelectShopContext();
            IEnumerable<TWordCensorship> a = db.TWordCensorships.OrderByDescending(x=>x.WordCensorshipId);
            if (!string.IsNullOrEmpty(keyword))
                a =a.Where(x => x.Word.ToLower().Contains(keyword.ToLower()));
            return View(a);
        }
        public IActionResult addWordCensorship2(string word)
        {
            SelectShopContext db = new SelectShopContext();
            TWordCensorship wordCensorship = new TWordCensorship();
            wordCensorship.Word = word;
            wordCensorship.WordSeverityId = 1;
            db.TWordCensorships.Add(wordCensorship);
            db.SaveChanges();
            return RedirectToAction("WordCensorship2");
        }
        public IActionResult delectWordCensorshipAPI(int id)
        {
            SelectShopContext db = new SelectShopContext();
            int v = db.TWordCensorships.Where(x => x.WordCensorshipId == id).ExecuteDelete();
            return Content("ok", "text/plain");
        }
    }
}
