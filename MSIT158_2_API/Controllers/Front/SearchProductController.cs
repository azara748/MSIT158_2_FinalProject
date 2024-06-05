using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;

namespace MSIT158_2_API.Controllers.Front
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchProductController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public SearchProductController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/SearchProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProduct>>> GetTProducts()
        {
            return await _context.TProducts.ToListAsync();
        }

        // GET: api/SearchProduct/5
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

        // PUT: api/SearchProduct/5
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

		// POST: api/SearchProduct
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPost]
		//public async Task<ActionResult<TProduct>> PostTProduct(TProduct tProduct)
		//{
		//    _context.TProducts.Add(tProduct);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetTProduct", new { id = tProduct.ProductId }, tProduct);
		//}
		[HttpPost]
		public async Task<ActionResult<IEnumerable<TProduct>>> GetProductBySearch(SearchProductDTO searchProductDTO)
		{ //次分類
		  //var query = searchProductDTO.SubCatId == 0 ? _context.TProducts : _context.TProducts.Where(s => s.SubCategoryId == searchProductDTO.SubCatId);
		  //主分類
		  //query = searchProductDTO.CatId == 0 ? _context.TProducts : _context.TProducts..Where(s => s.SubCategoryId == searchProductDTO.SubCatId);
		  //品牌
		  //query = searchProductDTO.LabelId == 0 ? _context.TProducts : _context.TProducts.Where(s => s.LabelId == searchProductDTO.LabelId);
		  //活動
		  //query = searchProductDTO.ActiveId == 0 ? _context.TProducts : _context.TProducts.Where(s => s.ActiveId == searchProductDTO.ActiveId);
		  // 使用 AsQueryable 來延遲查詢
		  //query = _context.TProducts.AsQueryable();
		  // searchword查詢
		  //if (!string.IsNullOrEmpty(searchProductDTO.searchword))
		  //{
		  //	query = query.Where(p => p.ProductName.Contains(searchProductDTO.searchword));
		  //}

			//         var query = searchProductDTO.SubCatId == 0 ? _context.TProducts : _context.TProducts.Where(s => s.SubCategoryId == searchProductDTO.SubCatId);
			//         if (!string.IsNullOrEmpty(searchProductDTO.searchword))
			//         {
			//             query = query.Where(p => p.ProductName.Contains(searchProductDTO.searchword));
			//         }

			//         var getphoto = query.Select(p => p.ProductPhoto != null ? Convert.ToBase64String(ProductPhoto) : null);



			//ProductPhotoDTO photoDTO = new ProductPhotoDTO();
			//         photoDTO.result = await query.ToListAsync();
			//         photoDTO.photo = getphoto;
			//return Ok(photoDTO);
			var query = searchProductDTO.SubCatId == 0 ? _context.TProducts : _context.TProducts.Where(s => s.SubCategoryId == searchProductDTO.SubCatId);
			if (!string.IsNullOrEmpty(searchProductDTO.searchword))
			{
				query = query.Where(p => p.ProductName.Contains(searchProductDTO.searchword));
			}

			var products = await query.ToListAsync();
			return Ok(products);
			//var productPhotos = products.Select(p => new ProductPhotoDTO
			//{
			//	photo = p.ProductPhoto != null ? Convert.ToBase64String(p.ProductPhoto) : null
			//}).ToList();

			//return Ok(productPhotos);
		}
	


		// DELETE: api/SearchProduct/5
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
