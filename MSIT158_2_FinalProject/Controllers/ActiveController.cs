using Microsoft.AspNetCore.Mvc;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    public class ActiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
         public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TActive p)
        {
            SelectShopContext db = new SelectShopContext();
            db.TActives.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            SelectShopContext db = new SelectShopContext();
            TActive a = db.TActives.FirstOrDefault(x => x.ActiveId == id);
            if (a == null)
                return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        public IActionResult Edit(TActive a)
        {
            SelectShopContext db = new SelectShopContext();
            TActive activedb = db.TActives.FirstOrDefault(x => x.ActiveId == a.ActiveId);
            if (activedb != null)
            {
                activedb.ActiveName = a.ActiveName;
                activedb.StartDate = a.StartDate;
                activedb.EndDate = a.EndDate;
                activedb.Discount = a.Discount;
                activedb.Description = a.Description;
                activedb.ActiveImage = a.ActiveImage;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                SelectShopContext db = new SelectShopContext();
                TActive r = db.TActives.FirstOrDefault(x => x.ActiveId == id);
                if (r != null)
                {
                    db.TActives.Remove(r);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
