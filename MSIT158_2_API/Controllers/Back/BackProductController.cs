using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;

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


        //後台CRUD功能
        //新增 接受產品DTO之後傳入 ??
        //修改 用id讀出資料回傳 API 接受id->回傳資料
        //查詢 接受條件->回傳資料
        //刪除 找出id 刪掉 打勾都刪掉


        [HttpPost("SearchBackProduct")]
        public async Task<ActionResult<ShowProductDTO>> PostTProduct(SearchProductDTO searchProductDTO)
        {
            var query = _context.TProducts
                        .Include(p => p.SubCategory)
                        .Include(p => p.Active)
                        .Include(p => p.Label)
                        .Include(p => p.TPurchases)
                        .Include(p => p.TReviews)
                        .AsQueryable();
            //搜尋_次分類
            query = searchProductDTO.subcatId == 0 ? query : query.Where(s => s.SubCategoryId == searchProductDTO.subcatId);
            //搜尋_關鍵字
            if (!string.IsNullOrEmpty(searchProductDTO.searchword))
            {
                query = query.Where(p => p.ProductName.Contains(searchProductDTO.searchword)
                || p.SubCategory.SubCategoryCname.Contains(searchProductDTO.searchword)
                || p.Description.Contains(searchProductDTO.searchword));
            }
            //金額條件
            //if (searchProductDTO.lowPrice != null && searchProductDTO.highPrice != null)
            //{
            //    query = query.Where(p => p.UnitPrice > searchProductDTO.lowPrice && p.UnitPrice < searchProductDTO.highPrice);
            //}
            if (searchProductDTO.stock)
            {
                query = query.Where(p => p.Stocks > 0);
            }

            //if (searchProductDTO.rankfour)
            //{

            //    query = query.Where(p => p.TReviews.Average(x => x.RankId) >= 4);


            //}
            //if (searchProductDTO.rankthree)
            //{
            //    query = query.Where(p => p.TReviews.Average(x => x.RankId) >= 3);

            //}


            //排序_
            switch (searchProductDTO.sortBy)
                {
                case "ProductID":
                    query = searchProductDTO.sortType == "asc" ? query.OrderBy(s => s.ProductId) : query.OrderByDescending(s => s.ProductId);
                    break;
                case "UnitPrice":
                        query = searchProductDTO.sortType == "asc" ? query.OrderBy(s => s.UnitPrice) : query.OrderByDescending(s => s.UnitPrice);
                        break;

                    case "bestsale":
                        query = searchProductDTO.sortType == "asc" ?
                                        query.OrderBy(s => s.TPurchases.Sum(p => p.Qty)) :
                                        query.OrderByDescending(s => s.TPurchases.Sum(p => p.Qty));
                        break;
                    case "bestscore":
                        query = searchProductDTO.sortType == "asc" ?
                                        query.OrderBy(s => s.TReviews.Average(p => p.RankId)) :
                                        query.OrderByDescending(s => s.TReviews.Average(p => p.RankId));
                        break;
                    case "coldpro":
                        query = searchProductDTO.sortType == "asc" ?
                                        query.OrderByDescending(s => s.Stocks) : query.OrderBy(s => s.Stocks);
                        break;
                    default:
                        query = searchProductDTO.sortType == "asc" ?
                    query.OrderByDescending(s => s.TReviews.Average(p => p.RankId)) : query.OrderBy(s => s.TReviews.Average(p => p.RankId));
                        break;
                        //default:
                        //	query = searchProductDTO.sortType == "asc" ? query.OrderBy(s => s.ProductId) : query.OrderByDescending(s => s.ProductId);
                        //	break;
                }



                //計算評分	
                int totalCount = query.Count();
                int pageSize = searchProductDTO.pagesSize;
                int page = searchProductDTO.page;
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                query = query.Skip((page - 1) * pageSize).Take(pageSize);

                var showProductDTOs = query.Select(pro => new ShowProductDTO
                {
                    ProductId = pro.ProductId,
                    ProductName = pro.ProductName,
                    SubCategoryId = pro.SubCategoryId,
                    SubCatName = pro.SubCategory.SubCategoryCname,
                    Stocks = pro.Stocks,
                    UnitPrice = pro.UnitPrice,
                    Discount = pro.Active.Discount,
                    LabelName = pro.Label.LabelName,
                    Productphoto = pro.ProductPhoto,
                    Score = pro.TReviews.Average(p => p.RankId),
                    cost = pro.Cost,
                    Description = pro.Description,
                    LanchTime = pro.LaunchTime.HasValue ? pro.LaunchTime.Value.ToString("yyyy-MM-dd") : null
                }).ToList();

                var toClientProduct = new ToClientProductDTO
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    showProducts = showProductDTOs
                };

                return Ok(toClientProduct);

            }

        //[HttpPost("getName")]
        //public async Task<ActionResult<getNameDTO>> getIDName(int labelid, int subid)
        //{
        //       var query = _context.TProducts
        //                    .Include(s=>s.SubCategory)
        //                    .Include(s=>s.Label)
        //                    .AsQueryable();
        //    if(labelid > 0 && subid > 0)
        //    {
        //        var subName = query.Where(x => x.SubCategoryId == subid);
        //        var labelName = query.Where(x => x.LabelId == labelid);

        //    }

        //    var getNameDTO = new getNameDTO
        //    {
        //        subName = query.SubCategory.SubCategoryCname,
        //        labelName = query.Label.LabelName,
        //    };
        //    return getNameDTO;
        //}

        [HttpPost("AddBackProduct")]
        public async Task<ActionResult<ShowProductDTO>> AddTProduct(AddProductDTO addProductDTO)
        {
            // 將 Base64 字串轉換為二進位數據
            byte[] imageBytes = Convert.FromBase64String(addProductDTO.Productphoto);

            ////addProductDTO.LanchTime指回傳Dateonly 加上現在時間之後 傳回TProduct裡面Lanchtime(類別為DateTime)
            //TimeSpan currentTime = DateTime.Now.TimeOfDay;
            //   DateOnly dateOnly = addProductDTO.LanchTime.Value;
            //    DateTime dateTimeWithCurrentTime = dateOnly.ToDateTime(currentTime);

            TProduct newtp = new TProduct
            {
                ProductPhoto = imageBytes,
                ProductName = addProductDTO.ProductName,
                //SubCatName = AddProductDTO.SubCategory.SubCategoryCname,
                Stocks = addProductDTO.Stocks,
                LabelId = addProductDTO.LabelId,
                SubCategoryId = addProductDTO.SubCategoryId,
                UnitPrice = addProductDTO.UnitPrice,
                Cost = addProductDTO.cost,
                Description = addProductDTO.Description,
                LaunchTime = addProductDTO.LanchTime
            };

            

            _context.TProducts.Add(newtp);
            await _context.SaveChangesAsync();
            return Ok("addProduct");
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
