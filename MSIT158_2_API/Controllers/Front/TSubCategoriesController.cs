using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;

namespace MSIT158_2_API.Controllers.Front
{
    [Route("api/[controller]")]
    [ApiController]
    public class TSubCategoriesController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public TSubCategoriesController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/TSubCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TSubCategory>>> GetTSubCategories()
        {
            return await _context.TSubCategories.ToListAsync();
        }

        // GET: api/TSubCategories/5
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

        // PUT: api/TSubCategories/5
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

        // POST: api/TSubCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TSubCategory>> PostTSubCategory(TSubCategory tSubCategory)
        {
            _context.TSubCategories.Add(tSubCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTSubCategory", new { id = tSubCategory.SubCategoryId }, tSubCategory);
        }

        // DELETE: api/TSubCategories/5
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
