using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;
using System.Web;

namespace MSIT158_2_FinalProject.Controllers
{
    public class ActiveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ActiveImage photo, HttpPostedFileBase image)
        {   //                                                           ****************************
            if (ModelState.IsValid)
            {   //*** 檔案上傳 ****************************************(start)
                if (image != null)
                {
                    photo.PhotoFile = new byte[image.ContentLength];
                    image.InputStream.Read(photo.PhotoFile, 0, image.ContentLength);
                }
                //*** 檔案上傳 ****************************************(end)
                SelectShopContext db = new SelectShopContext();
                _db.Photos.Add(photo);   // 新增一筆記錄
                _db.SaveChanges();   // 正式寫入資料庫！

                return RedirectToAction("ImageIndex");
            }

            return View(photo);
        }

        public ActionResult ImageIndex()
        {
            return View("ImageIndex", _db.Photos.ToList());
        }

        //*** 把資料表裡面的「二進位」內容，還原成圖片檔 ****************************
        public FileContentResult GetImage(int PhotoID)
        {
            Photo requestedPhoto = _db.Photos.FirstOrDefault(p => p.PhotoID == PhotoID);


            if (requestedPhoto != null)
            {
                return File(requestedPhoto.PhotoFile, "image/jpeg");
            }
            else
            {
                return null;
            }
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
