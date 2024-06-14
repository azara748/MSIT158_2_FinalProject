using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.ViewModel;

namespace MSIT158_2_FinalProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly SelectShopContext _context;
        public EmployeeController(SelectShopContext context) 
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            TEmployee r = _context.TEmployees.FirstOrDefault(x => x.EmployeeId == id);
            if (r == null)
                return RedirectToAction("List");
            return View(r);
        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                TEmployee r = _context.TEmployees.FirstOrDefault(x => x.EmployeeId == id);
                if (r != null)
                {
                    _context.TEmployees.Remove(r);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        public IActionResult Details(int id) 
        {
            if (id == null)
                return RedirectToAction("List");
            TEmployee r = _context.TEmployees.FirstOrDefault(x => x.EmployeeId == id);
            if (r == null)
                return RedirectToAction("List");
            return View(r);
        }
    }
}
