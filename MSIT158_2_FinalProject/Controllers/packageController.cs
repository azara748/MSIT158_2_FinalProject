using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    public class packageController : Controller
    {
        private readonly SelectShopContext _context;
        public packageController(SelectShopContext context)
        {
            _context = context;
        }

        public ActionResult AllPackages()
        {
            var allPackage = _context.TAllPackages.Select(p =>new { p.PackName,p.Picture,p.Price });
            return Json(allPackage);
        }

        public ActionResult PackageCategory()
        {
           
        }

        public ActionResult AllPackagesPage()
        {
            return View();
        }

        // GET: packageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: packageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: packageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: packageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: packageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: packageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: packageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
