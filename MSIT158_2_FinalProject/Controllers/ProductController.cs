using Microsoft.AspNetCore.Mvc;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private IWebHostEnvironment _enviro = null;
        public ProductController(IWebHostEnvironment p)
        {
            _enviro = p;
        }

        public IActionResult List()
        {
            SelectShopContext db = new SelectShopContext();
            IEnumerable<TProduct> datas = null;
            datas = from t in db.TProducts
                    select t;
            return View(datas);

        }
        public IActionResult BList()
        {
            return View();

        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TProduct p)
        {
            SelectShopContext db = new SelectShopContext();
            db.TProducts.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                SelectShopContext db = new SelectShopContext();
                TProduct r = db.TProducts.FirstOrDefault(x => x.ProductId == id);
                if (r != null)
                {
                    //db.TProducts.Remove(r);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            SelectShopContext db = new SelectShopContext();
            TProduct r = db.TProducts.FirstOrDefault(x => x.ProductId == id);
            if (r == null)
                return RedirectToAction("List");
            return View(r);
        }
        [HttpPost]
        public ActionResult Edit(TProduct pw)
        {
            SelectShopContext db = new SelectShopContext();
            TProduct proDb = db.TProducts.FirstOrDefault(x => x.ProductId == pw.ProductId);
            if (proDb != null)
            {
                //if (pw.ProductPhoto != null)
                //{
                //    string photoName = Guid.NewGuid().ToString() + ".png";
                //    proDb.ProductPhoto = photoName;
                //    pw.ProductPhoto.CopyTo(new FileStream(_enviro.WebRootPath + "/image/" + photoName, FileMode.Create));
                //}
                proDb.ProductName = pw.ProductName;
                proDb.UnitPrice = pw.UnitPrice;
                proDb.Cost = pw.Cost;
                proDb.Stocks = pw.Stocks;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }

    //測試用Main更新??
}
