using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

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
		//public async Task<ActionResult<IEnumerable<TProduct>>> GetProductBySearch(SearchProductDTO searchProductDTO)
		//[HttpPost]
		//public async Task<ActionResult<IEnumerable<ShowProductDTO>>> GetProductBySearch(SearchProductDTO searchProductDTO)
		//{ //次分類
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
		//var query = searchProductDTO.SubCatId == 0 ? _context.TProducts : _context.TProducts.Where(s => s.SubCategoryId == searchProductDTO.SubCatId);




		//                            SubCatName = c.SubCategoryCname,
		//                   ProductName = p.ProductName,
		[HttpPost]
		public async Task<ActionResult<ShowProductDTO>> GetProductBySearch(SearchProductDTO searchProductDTO)
		{

			var query = _context.TProducts
						.Include(p => p.SubCategory)
						.Include(p => p.Active)
						.Include(p => p.Label)
						.Include(p => p.TPurchases)
						.AsQueryable();
			//搜尋_次分類
			query = searchProductDTO.subcatId == 0 ? query : query.Where(s => s.SubCategoryId == searchProductDTO.subcatId);
			//搜尋_關鍵字
			if (!string.IsNullOrEmpty(searchProductDTO.searchword))
			{
				query = query.Where(p => p.ProductName.Contains(searchProductDTO.searchword));
			}
			//排序_

			switch (searchProductDTO.sortBy)
			{
				case "UnitPrice":
					query = searchProductDTO.sortType == "asc" ? query.OrderBy(s => s.UnitPrice) : query.OrderByDescending(s => s.UnitPrice);
					break;

				case "bestsale":
					query = searchProductDTO.sortType == "asc" ?
									query.OrderBy(s => s.TPurchases.Sum(p => p.Qty)) :
									query.OrderByDescending(s => s.TPurchases.Sum(p => p.Qty));
					break;

				default:
					query = searchProductDTO.sortType == "asc" ? query.OrderBy(s => s.ProductId) : query.OrderByDescending(s => s.ProductId);
					break;
			}

			int totalCount = query.Count();
			int pageSize = searchProductDTO.pagesSize;
			int page = searchProductDTO.page;
			int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

			query = query.Skip((page - 1) * pageSize).Take(pageSize);


			//foreach (var pro in query) { 
			//	ShowProductDTO sp = new ShowProductDTO();
			//	sp.ProductName = pro.ProductName;
			//	sp.SubCategoryId = pro.SubCategoryId;
			//	sp.SubCatName = pro.SubCategory.SubCategoryCname;
			//	sp.Stocks = pro.Stocks;
			//	sp.LanchTime = pro.LaunchTime;
			//	sp.UnitPrice = pro.UnitPrice;
			//	sp.Discount = pro.Active.Discount;
			//	sp.LabelName = pro.Label.LabelName;
			//	sp.Productphoto = pro.ProductPhoto;
			//};
			var products = await query
	.Skip((page - 1) * pageSize)
	.Take(pageSize)
	.ToListAsync();

			var showProductDTOs = products.Select(pro => new ShowProductDTO
			{
				ProductName = pro.ProductName,
				SubCategoryId = pro.SubCategoryId,
				SubCatName = pro.SubCategory.SubCategoryCname,
				Stocks = pro.Stocks,
				LanchTime = pro.LaunchTime,
				UnitPrice = pro.UnitPrice,
				Discount = pro.Active.Discount,
				LabelName = pro.Label.LabelName,
				Productphoto = pro.ProductPhoto
			}).ToList();


			//ToClientProductDTO toClientProduct = new ToClientProductDTO();
			//toClientProduct.TotalCount = totalCount;
			//toClientProduct.TotalPages = totalPages;
			//toClientProduct.showProducts = await query.ToListAsync();

			var toClientProduct = new ToClientProductDTO
			{
				TotalCount = totalCount,
				TotalPages = totalPages,
				showProducts = showProductDTOs
			};

			return Ok(toClientProduct);

		}
		//LanchTime = !string.IsNullOrEmpty(p.LanchTime) ? p.LanchTime : "新上市",

		//         if (!string.IsNullOrEmpty(searchProductDTO.searchword))
		//         {
		//             query = query.Where(p => p.SubCategory.SubCategoryCname.Contains(searchProductDTO.searchword));
		//}

		//         ShowProductDTO sp = new ShowProductDTO();
		//         sp.tProduct = await _context.TProducts.ToListAsync();
		//         sp.tSubCategory = await _context.TSubCategories.ToListAsync();

		//var productList = await query.ToListAsync();

		//ShowProductDTO sp = new ShowProductDTO();
		//sp.ProductResult = await _context.TProducts.ToListAsync();
		//return Ok(sp);

		////總共有多少筆資料
		//int totalCount = query.Count();
		////每頁要顯示幾筆資料
		//int pageSize = searchProductDTO.pagesSize;
		////目前第幾頁
		//int page = searchProductDTO.page;
		////計算總共有幾頁
		//int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
		////分頁
		//query = query.Skip((page - 1) * pageSize).Take(pageSize);

		////包裝要傳給client端的資料
		//SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
		//spotsPaging.TotalCount = totalCount;
		//spotsPaging.TotalPages = totalPages;
		//spotsPaging.SpotsResult = await spots.ToListAsync();


		//var productPhotos = products.Select(p => new ProductPhotoDTO
		//{
		//	photo = p.ProductPhoto != null ? Convert.ToBase64String(p.ProductPhoto) : null
		//}).ToList();

		//return Ok(productPhotos);




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
