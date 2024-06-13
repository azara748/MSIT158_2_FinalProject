using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.Models.DTO;
using Newtonsoft.Json;

namespace MSIT158_2_FinalProject.Controllers
{
    public class packageController : Controller
    {
        private readonly SelectShopContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public packageController(SelectShopContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public ActionResult AllPackages()
        {
            var allPackage = _context.TAllPackages.Select(p => new { p.PackageId, p.PackName, p.Picture, p.Price });
            return Json(allPackage);
        }

        public ActionResult PackageCategory()
        {
            var category = _context.TPackageStyles.Select(c => new { c.PackageStyleId, c.StyleName }).ToList();
            return Json(category);
        }

        public ActionResult Material()
        {
            var allMaterials = _context.TPackageMaterials
       .Select(s => new { s.MaterialId, s.MaterialName })
       .ToList();  // 获取所有数据

            var uniqueMaterials = allMaterials
                .GroupBy(s => s.MaterialName)
                .Select(g => g.First())
                .ToList();

            if (!uniqueMaterials.Any())
            {
                return NotFound();
            }

            return Json(uniqueMaterials);

        }

        //[Route("api/materials")]
        //[HttpGet("{name}")]
        public ActionResult MaterialColor(string name)
        {
            
            Console.WriteLine($"Searching for material name: {name}");

            var pMaterialColor = _context.TPackageMaterials
                .Join(_context.TMaterialColors,
                    ma => ma.ColorId,
                    co => co.ColorId,
                    (ma, co) => new pMaColorClass
                    {
                        MaterialColorId = ma.ColorId,
                        MaterialName = ma.MaterialName,
                        ColorName = co.ColorName,
                    }).Where(p => p.MaterialName == name).ToList();

            if (pMaterialColor == null )
            {
                return NotFound();
            }

            return Json(pMaterialColor);
        }

        [HttpGet]
        public ActionResult<IEnumerable<packageMaterialName>> GetPackagesWithMaterialNames()
        {
            var packages = _context.TAllPackages
                .Join(_context.TPackageMaterials,
                    package => package.MaterialId,
                    material => material.MaterialId,
                    (package, material) => new packageMaterialName
                    {
                        PackageId = package.PackageId,
                        MaterialId = package.MaterialId,
                        MaterialName = material.MaterialName,
                        Description = package.Description,
                        Price = package.Price,
                        Picture = package.Picture,
                        PackageStyleId = package.PackageStyleId,
                        PackName = package.PackName,
                    })
                .ToList();

            return Ok(packages);
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
        public ActionResult EditPackage(int id)
        {
            //var package = _context.TAllPackages.Find(id);
            //if(package ==null)
            //{
            //    return NotFound();
            //}
            //return View(package);

            var package = _context.TAllPackages
                .Join(_context.TPackageMaterials,
                    package => package.MaterialId,
                    material => material.MaterialId,
                    (package, material) => new {package, material})
                .Join(_context.TMaterialColors,
                    pm =>pm.material.ColorId,
                    color => color.ColorId,
                    (pm, color) => new packageMaterialName
                    {
                        PackageId = pm.package.PackageId,
                        MaterialId = pm.package.MaterialId,
                        MaterialName = pm.material.MaterialName,
                        MaterialColorId= pm.material.ColorId,
                        ColorName = color.ColorName,
                        Description =  pm.package.Description,
                        Price = pm.package.Price,
                        Picture = pm.package.Picture,
                        PackageStyleId = pm.package.PackageStyleId,
                        PackName = pm.package.PackName,
                    }).FirstOrDefault(p => p.PackageId == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        //[HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromForm] string updatedPackage, [FromForm] IFormFile? avatar)
        {
            var updatedPackageData = JsonConvert.DeserializeObject<TAllPackage>(updatedPackage);
            if (id != updatedPackageData.PackageId)
            {
                return BadRequest();
            }

            var package = await _context.TAllPackages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }


            if (avatar != null)
            {
                // 设置文件上传路径为 wwwroot/assets/img/packageImages
                string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "assets", "img", "packageImages", avatar.FileName);

                // 确保目录存在
                //var directory = Path.GetDirectoryName(uploadPath);
                //if (!Directory.Exists(directory))
                //{
                //    Directory.CreateDirectory(directory);
                //}

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }

                //檔案上傳轉成二進位
                byte[]? imgByte = null;
                using (var memoryStream = new MemoryStream())
                {
                    await avatar.CopyToAsync(memoryStream);
                    imgByte = memoryStream.ToArray();
                }

                package.Picture = avatar.FileName; // 更新现有记录的图片文件名
                package.PictureData = imgByte; // 更新现有记录的图片数据
            }

            package.PackName = updatedPackageData.PackName;
            package.Price = updatedPackageData.Price;
            package.Description = updatedPackageData.Description;
            package.MaterialId = updatedPackageData.MaterialId;
            package.PackageStyleId = updatedPackageData.PackageStyleId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TAllPackages.Any(e => e.PackageId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
