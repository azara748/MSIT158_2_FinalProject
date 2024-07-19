using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TAllPackagesController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public TAllPackagesController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/TAllPackages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TAllPackage>>> GetTAllPackages()
        {
            return await _context.TAllPackages.ToListAsync();
        }

        // GET: api/TAllPackages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TAllPackage>> GetTAllPackage(int id)
        {
            var tAllPackage = await _context.TAllPackages.FindAsync(id);

            if (tAllPackage == null)
            {
                return NotFound();
            }

            return tAllPackage;
        }

        // PUT: api/TAllPackages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTAllPackage(int id, TAllPackage tAllPackage)
        {
            if (id != tAllPackage.PackageId)
            {
                return BadRequest();
            }

            _context.Entry(tAllPackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TAllPackageExists(id))
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

        // POST: api/TAllPackages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TAllPackage>> PostTAllPackage(TAllPackage tAllPackage)
        {
            _context.TAllPackages.Add(tAllPackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTAllPackage", new { id = tAllPackage.PackageId }, tAllPackage);
        }

        [HttpPost]
        [Route("selectID")]
        public async Task<ActionResult<IEnumerable<TAllPackage>>> PostTAllPackage(int seletId)
        {
            if (_context.TAllPackages == null)
            {
                return BadRequest("Context is null");
            }

            var selectedPackages = await _context.TAllPackages
                                                 .Where(s => s.PackageStyleId == seletId)
                                                 .ToListAsync();

            return Ok(selectedPackages);

        }

        // DELETE: api/TAllPackages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTAllPackage(int id)
        {
            var tAllPackage = await _context.TAllPackages.FindAsync(id);
            if (tAllPackage == null)
            {
                return NotFound();
            }

            _context.TAllPackages.Remove(tAllPackage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TAllPackageExists(int id)
        {
            return _context.TAllPackages.Any(e => e.PackageId == id);
        }
    }
}
