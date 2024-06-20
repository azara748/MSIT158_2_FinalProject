using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;
using MSIT158_2_API.Models.DTO.Back;

namespace MSIT158_2_API.Controllers.Back
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackProductController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public BackProductController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/BackProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProduct>>> GetTProducts()
        {
            return await _context.TProducts.ToListAsync();
        }

        // GET: api/BackProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TProduct>> GetTProduct(int id)
        {
            var tProduct = await _context.TProducts.FindAsync(id);

            if (tProduct == null)
            {
                return NotFound();
            }

            return tProduct;
        }

        // PUT: api/BackProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTProduct(int id, TProduct tProduct)
        {
            if (id != tProduct.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(tProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TProductExists(id))
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


        // POST: api/BackProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<TProduct>> PostTProduct(TProduct tProduct)
        //{
        //    _context.TProducts.Add(tProduct);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTProduct", new { id = tProduct.ProductId }, tProduct);
        //}

        //商品清單
        [HttpPost("SearchBackProduct")]
        public async Task<ActionResult<ProductShowDTO>> PostTProduct(ProductSearchDTO ProductsearchDTO)
        {
            Console.WriteLine(ProductsearchDTO);
            var query = _context.TProducts
                        .Include(p => p.SubCategory)
                        .Include(p => p.Active)
                        .Include(p => p.Label)
                        .Include(p => p.TPurchases)
                        .Include(p => p.TReviews)
                        .AsQueryable();
            //搜尋_品牌
            query = ProductsearchDTO.labelID == 0 ? query : query.Where(s => s.LabelId == ProductsearchDTO.labelID); 
            //搜尋_次分類
            query = ProductsearchDTO.catID == 0 ? query : query.Where(s => s.SubCategoryId == ProductsearchDTO.catID);

            //搜尋_關鍵字
            if (!string.IsNullOrEmpty(ProductsearchDTO.searchword))
            {
                query = query.Where(p => p.ProductName.Contains(ProductsearchDTO.searchword)
                || p.SubCategory.SubCategoryCname.Contains(ProductsearchDTO.searchword)
                || p.Description.Contains(ProductsearchDTO.searchword));
            }

            //搜尋_ID
            if (ProductsearchDTO.proId !=0)
            {
                query = query.Where(p => p.ProductId ==  ProductsearchDTO.proId);
            }

            //排序_
            switch (ProductsearchDTO.sortBy)
            {
                case "proID":
                    query = ProductsearchDTO.sortType == "asc" ? query.OrderBy(s => s.ProductId) : query.OrderByDescending(s => s.ProductId);
                    break;
                default:
                    query = ProductsearchDTO.sortType == "asc" ? query.OrderBy(s => s.ProductId) : query.OrderByDescending(s => s.ProductId);
                    break;
            }
            if (ProductsearchDTO.status==1)
            {
                   query = query.Where(p => p.Status == 1);
            }
            if (ProductsearchDTO.status == 2)
            {
                query = query.Where(p => p.Status == 2);
            }
            int totalCount = query.Count();
                int pageSize = ProductsearchDTO.pagesSize;
                int page = ProductsearchDTO.page;
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                query = query.Skip((page - 1) * pageSize).Take(pageSize);

                var ProductShows = query.Select(pro => new ProductShowDTO
                {
                    ProductId = pro.ProductId,
                    ProductName = pro.ProductName,
                    SubCategoryId = pro.SubCategoryId,
                    SubCatName = pro.SubCategory.SubCategoryCname,
                    Stocks = pro.Stocks,
                    UnitPrice = pro.UnitPrice,
                    cost = pro.Cost,
                    LabelId = pro.LabelId,
                    LabelName = pro.Label.LabelName,
                    LanchTime = pro.LaunchTime,
                    Productphoto = pro.ProductPhoto,
                    Description = pro.Description,
                    Status = pro.Status
                }).ToList();

                var toBackProduct = new ProductToBackDTO
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    Productshow = ProductShows
                };

                return Ok(toBackProduct);

            }
        //新增商品
        [HttpPost("AddBackProduct")]
        public async Task<ActionResult<ProductShowDTO>> AddTProduct(AddProductDTO addProductDTO)
        {
            try
            {

                // 將 Base64 字串轉換為二進位數據
                byte[] imageBytes = Convert.FromBase64String(addProductDTO.Productphoto);

            TProduct newtp = new TProduct
            {
                ProductPhoto = imageBytes,
                ProductName = addProductDTO.ProductName,
                Stocks = addProductDTO.Stocks,
                LabelId = addProductDTO.LabelId,
                SubCategoryId = addProductDTO.SubCategoryId,
                UnitPrice = addProductDTO.UnitPrice,
                Cost = addProductDTO.Cost,
                Description = addProductDTO.Description,
                LaunchTime = addProductDTO.LanchTime,
                Status = addProductDTO.status
            };

            _context.TProducts.Add(newtp);
            await _context.SaveChangesAsync();
            return Ok(newtp.ProductId);
            }
            catch (FormatException ex)
            {
                // 捕获 Base64 转换中的格式错误
                return BadRequest("Invalid Base64 string: " + ex.Message);
            }
        }
        //ID找出該筆資料 一筆就好
        [HttpPost("IDFindBackProduct")]
        public async Task<ActionResult<ProductShowDTO>> IDFindTProduct(int id)
        {
            //用ID找出Tproduct資料 回傳我要的
            var query = _context.TProducts
                                    .Include(p => p.SubCategory)
                                    .Include(p => p.Active)
                                    .Include(p => p.Label)
                                    .Include(p => p.TPurchases)
                                    .Include(p => p.TReviews)
                                    .AsQueryable();
            query = query.Where(x => x.ProductId == id);

            var idtp = query.Select(pro => new ProductShowDTO
            {
                ProductId = pro.ProductId,
                ProductName = pro.ProductName,
                Stocks = pro.Stocks,
                SubCategoryId = pro.SubCategoryId,
                SubCatName = pro.SubCategory.SubCategoryCname,
                LabelId = pro.LabelId,
                LabelName = pro.Label.LabelName,
                UnitPrice = pro.UnitPrice,
                cost = pro.Cost,
                Description = pro.Description,
                Productphoto = pro.ProductPhoto,
                LanchTime = pro.LaunchTime,
                Status = pro.Status
            }).ToList();

            return Ok(idtp);

        }
        //更新資料回傳
        [HttpPost("UpdateBackProduct")]
        public async Task<ActionResult<ProductShowDTO>> UpdateTProduct(int id, AddProductDTO addProductDTO)
        {
            //把資料庫裡面那一筆的資料抓出來
            var product = await _context.TProducts.FirstOrDefaultAsync(x => x.ProductId == id);
            //改新的
            byte[] imageBytes = Convert.FromBase64String(addProductDTO.Productphoto);

            // 更新產品資料
            product.ProductName = addProductDTO.ProductName;
            product.UnitPrice = addProductDTO.UnitPrice;
            product.Stocks = addProductDTO.Stocks;
            product.SubCategoryId = addProductDTO.SubCategoryId;
            product.LabelId = addProductDTO.LabelId;
            product.Cost = addProductDTO.Cost;
            product.Description = addProductDTO.Description;
            product.ProductPhoto = imageBytes;
            product.LaunchTime =addProductDTO.LanchTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok("修改成功");
            
        }
        //刪除資料(改變狀態)
        [HttpPost("DownBackProduct")]
        public async Task<ActionResult<ProductShowDTO>> DownTProduct(int id)
        {

            var query = await _context.TProducts.FirstOrDefaultAsync(p => p.ProductId == id);
            if(query.Status == 1)
            {
                query.Status = 2;
                await _context.SaveChangesAsync();
                return Ok("商品已下架!!");
            }
            else
            {
                query.Status = 1;
                await _context.SaveChangesAsync();
                return Ok("商品已上架!!");
            }


        }
        //[HttpPost]
        //public async Task<ActionResult<TProduct>> PostTProduct(TProduct tProduct)
        //{
        //    _context.TProducts.Add(tProduct);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTProduct", new { id = tProduct.ProductId }, tProduct);
        //}

        // DELETE: api/BackProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTProduct(int id)
        {
            var tProduct = await _context.TProducts.FindAsync(id);
            if (tProduct == null)
            {
                return NotFound();
            }

            _context.TProducts.Remove(tProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TProductExists(int id)
        {
            return _context.TProducts.Any(e => e.ProductId == id);
        }
    }
}
