using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;

namespace MSIT158_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TPackageStylesController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public TPackageStylesController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/TPackageStyles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TPackageStyle>>> GetTPackageStyles()
        {
            return await _context.TPackageStyles.ToListAsync();
        }

        // GET: api/TPackageStyles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TPackageStyle>> GetTPackageStyle(int id)
        {
            var tPackageStyle = await _context.TPackageStyles.FindAsync(id);

            if (tPackageStyle == null)
            {
                return NotFound();
            }

            return tPackageStyle;
        }

        // PUT: api/TPackageStyles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTPackageStyle(int id, TPackageStyle tPackageStyle)
        {
            if (id != tPackageStyle.PackageStyleId)
            {
                return BadRequest();
            }

            _context.Entry(tPackageStyle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TPackageStyleExists(id))
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

        // POST: api/TPackageStyles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TPackageStyle>> PostTPackageStyle(TPackageStyle tPackageStyle)
        {
            _context.TPackageStyles.Add(tPackageStyle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTPackageStyle", new { id = tPackageStyle.PackageStyleId }, tPackageStyle);
        }

        // DELETE: api/TPackageStyles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTPackageStyle(int id)
        {
            var tPackageStyle = await _context.TPackageStyles.FindAsync(id);
            if (tPackageStyle == null)
            {
                return NotFound();
            }

            _context.TPackageStyles.Remove(tPackageStyle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TPackageStyleExists(int id)
        {
            return _context.TPackageStyles.Any(e => e.PackageStyleId == id);
        }
    }
}
