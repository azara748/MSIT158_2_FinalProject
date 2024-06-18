using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    public class ValuablePeopleController : Controller
    {
        private readonly SelectShopContext _context;
        public ValuablePeopleController(SelectShopContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            TVip r = _context.TVips.FirstOrDefault(x => x.Vipid == id);
            if (r == null)
                return RedirectToAction("List");
            return View(r);
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                TVip r = _context.TVips.FirstOrDefault(x => x.Vipid == id);
                if (r != null)
                {
                    _context.TVips.Remove(r);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}
