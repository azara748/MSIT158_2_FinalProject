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
    public class SubcatDTOController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public SubcatDTOController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/SubcatDTO
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TSubCategory>>> GetTSubCategories()
        {
            return await _context.TSubCategories.ToListAsync();
        }

        // GET: api/SubcatDTO/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TSubCategory>> GetTSubCategory(int id)
        {
            var tSubCategory = await _context.TSubCategories.FindAsync(id);

            if (tSubCategory == null)
            {
                return NotFound();
            }

            return tSubCategory;
        }

        // PUT: api/SubcatDTO/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTSubCategory(int id, TSubCategory tSubCategory)
        {
            if (id != tSubCategory.SubCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(tSubCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TSubCategoryExists(id))
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

        // POST: api/SubcatDTO
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TSubCategory>> PostTSubCategory(SubcatDTO subcatDTO)
        {
            TSubCategory subcat = new TSubCategory();
			//TSubCategory t = new TSubCategory();
			_context.TSubCategories.Add(subcat);
			await _context.SaveChangesAsync();

            subcatDTO.SubCategoryId = subcat.SubCategoryId;
            subcatDTO.SubCategoryCname = subcat.SubCategoryCname;

			return CreatedAtAction("GetCategory", new { id = subcatDTO.SubCategoryId }, subcatDTO);

		}	

        // DELETE: api/SubcatDTO/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTSubCategory(int id)
        {
            var tSubCategory = await _context.TSubCategories.FindAsync(id);
            if (tSubCategory == null)
            {
                return NotFound();
            }

            _context.TSubCategories.Remove(tSubCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TSubCategoryExists(int id)
        {
            return _context.TSubCategories.Any(e => e.SubCategoryId == id);
        }
    }
}
