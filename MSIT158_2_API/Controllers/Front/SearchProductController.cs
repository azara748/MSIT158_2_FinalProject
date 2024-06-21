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
using Microsoft.IdentityModel.Tokens;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        [HttpPost]
		public async Task<ActionResult<ShowProductDTO>> GetProductBySearch(SearchProductDTO searchProductDTO)
		{

			var query = _context.TProducts.Where(p => p.Status ==1)
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
				||p.SubCategory.SubCategoryCname.Contains(searchProductDTO.searchword)
				||p.Description.Contains(searchProductDTO.searchword));
			}
			//金額條件
			if(searchProductDTO.lowPrice!=null && searchProductDTO.highPrice!=null)
			{
				query = query.Where(p => p.UnitPrice > searchProductDTO.lowPrice && p.UnitPrice < searchProductDTO.highPrice);
			}
			if (searchProductDTO.stock)
			{
				query = query.Where(p => p.Stocks > 0);
			}
			if (searchProductDTO.newlan)
			{
				//var currentDate = DateTime.Now;
                query = query.Where(p => p.LaunchTime.HasValue && EF.Functions.DateDiffDay(p.LaunchTime, currentDate) < 30);

			}

            if (searchProductDTO.rankfour)
			{
				query = query.Where(p => p.TReviews.Average(x => x.RankId)>=4);
			}
			if (searchProductDTO.rankthree)
			{
				query = query.Where(p => p.TReviews.Average(x => x.RankId) >= 3);
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
				case "bestscore":
					query = searchProductDTO.sortType == "asc" ?
									query.OrderBy(s => s.TReviews.Average(p => p.RankId)) :
									query.OrderByDescending(s => s.TReviews.Average(p => p.RankId));
					break;
				case "coldpro":
					query = searchProductDTO.sortType == "asc" ?
									query.OrderByDescending(s => s.Stocks) :query.OrderBy(s => s.Stocks) ;
					break;
				default:
					query = searchProductDTO.sortType == "asc" ? 
				query.OrderByDescending(s => s.TReviews.Average(p => p.RankId)) :  query.OrderBy(s => s.TReviews.Average(p => p.RankId));
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
				ProductId=pro.ProductId,
				ProductName = pro.ProductName,
				SubCategoryId = pro.SubCategoryId,
				SubCatName = pro.SubCategory.SubCategoryCname,
				Stocks = pro.Stocks,
				UnitPrice = pro.UnitPrice,
				Discount = pro.Active.Discount,
				LabelName = pro.Label.LabelName,
				Productphoto = pro.ProductPhoto,
				Score = pro.TReviews.Average(p => p.RankId),
				isnew = pro.LaunchTime.HasValue && EF.Functions.DateDiffDay( pro.LaunchTime.Value,currentDate) < 30
			}).ToList();

			var toClientProduct = new ToClientProductDTO
			{
				TotalCount = totalCount,
				TotalPages = totalPages,
				showProducts = showProductDTOs
			};

			return Ok(toClientProduct);

		}

		//[HttpPost("TOP3SalePro")]
		//public async Task<ActionResult<ShowProductDTO>> TOP3SalePro()
		//{
		//	var query = _context.TProducts.Include(p => p.TPurchases).AsQueryable();
		//	query = query.Take(3).OrderByDescending(s => s.TPurchases.Sum(p => p.Qty));
		//	var showProductDTOs = new ShowProductDTO
		//	{ 
		//		ProductName = query.ProductName,
		//		Productphoto = query.ProductPhoto
		//	}
		//	return Ok(showProductDTOs);
		//}
		[HttpPost("TOP3SalePro")]
		public async Task<ActionResult<List<ShowProductDTO>>> TOP3SalePro()
		{
			var query = _context.TProducts
				.Include(p => p.TPurchases)
				.Select(p => new
				{
					Product = p,
					TotalQty = p.TPurchases.Sum(pur => pur.Qty)
				})
				.OrderByDescending(x => x.TotalQty)
				.Take(3);

			var result = await query
				.Select(x => new ShowProductDTO
				{
					ProductName = x.Product.ProductName,
					Productphoto = x.Product.ProductPhoto,
					ProductId = x.Product.ProductId,
				})
				.ToListAsync();

			return Ok(result);
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

        //static int CalculateDaysUntilBirthday(string currentDate, string birthday)
        //{
        //    DateTime current = DateTime.ParseExact(currentDate, "yyyy/MM/dd", null);
        //    DateTime nextBirthday = DateTime.ParseExact(birthday, "yyyy/MM/dd", null);

        //    // Adjust the next birthday to the current year
        //    nextBirthday = new DateTime(current.Year, nextBirthday.Month, nextBirthday.Day);

        //    // Calculate the difference in days
        //    if (nextBirthday < current)
        //    {
        //        nextBirthday = nextBirthday.AddYears(1); // If next birthday has passed, add one year
        //    }

        //    TimeSpan difference = nextBirthday - current;
        //    int days = (int)difference.TotalDays;

        //    return days;
        //}
    }
}
