using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    public class ActiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TActive active, IFormFile myimg)
        {
            if (ModelState.IsValid)
            {
                if (myimg != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        myimg.CopyTo(ms);
                        active.ActiveImage = ms.ToArray();
                    }
                }
                _context.Add(active);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categories"] = new SelectList(_context.Set<TActive>(), "Id", "Name", active.ActiveId);
            return View(active);
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
            return View(a);
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

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase File)
        {
            if (File != null && File.ContentLength > 0)
            {
                //存到資料夾
                var FileName = Path.GetFileName(File.FileName);
                var FilePath = Path.Combine(Server.MapPath("~/Images/"), FileName);
                File.SaveAs(FilePath);


                //轉成byte 方法一 直接轉
                byte[] FileBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    File.InputStream.CopyTo(ms);
                    FileBytes = ms.GetBuffer();
                }
                //方法二 讀實體檔案出來再轉
                //using (var Fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                //{
                //    using (var Reader = new BinaryReader(Fs))
                //    {
                //        FileBytes = Reader.ReadBytes((int)Fs.Length);
                //    }
                //}

                //存進資料庫再取出
                InsertPhoto(FileBytes);
                FileBytes = SelectPhoto();

                TempData["Data"] = FileBytes;

            }


            return View();
        }
    }
}
