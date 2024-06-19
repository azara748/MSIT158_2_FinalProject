using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    public class ValuableCustomerController : Controller
    {
        private readonly SelectShopContext _context;
        public ValuableCustomerController(SelectShopContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            IEnumerable<TVip> datas = null;
            datas = from v in _context.TVips
                    select v;
            return View(datas);
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
        [HttpPost]
        public IActionResult Edit(TVip vipIn, IFormFile avatar)
        {
            //檔案上傳轉成二進位
            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                avatar.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            TVip vipDb = _context.TVips.FirstOrDefault(x => x.Vipid == vipIn.Vipid);
            if (vipDb != null)
            {
                vipDb.Vipname = vipIn.Vipname;
                vipDb.Vipphoto = imgByte;
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                //TVip r = _context.TVips.FirstOrDefault(x => x.Vipid == id);
                //if (r != null)
                //{
                //    _context.TVips.Remove(r);
                //    _context.SaveChanges();
                //}
            }
            return RedirectToAction("List");
        }
    }
}
