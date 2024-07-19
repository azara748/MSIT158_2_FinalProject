using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;

namespace MSIT158_2_FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TPackageCartsController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public TPackageCartsController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/TPackageCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TPackageCart>>> GetTPackageCarts()
        {
            return await _context.TPackageCarts.ToListAsync();
        }

        // GET: api/TPackageCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TPackageCart>> GetTPackageCart(int id)
        {
            var tPackageCart = await _context.TPackageCarts.FindAsync(id);

            if (tPackageCart == null)
            {
                return NotFound();
            }

            return tPackageCart;
        }

        // PUT: api/TPackageCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTPackageCart(int id, TPackageCart tPackageCart)
        {
            if (id != tPackageCart.PackageCartId)
            {
                return BadRequest();
            }

            _context.Entry(tPackageCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TPackageCartExists(id))
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

        //POST: api/TPackageCarts
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<TPackageCart>> PostTPackageCart(TPackageCart tPackageCart)
        //{
        //    _context.TPackageCarts.Add(tPackageCart);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTPackageCart", new { id = tPackageCart.PackageCartId }, tPackageCart);
        //}

        // DELETE: api/TPackageCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTPackageCart(int id)
        {
            var tPackageCart = await _context.TPackageCarts.FindAsync(id);
            if (tPackageCart == null)
            {
                return NotFound();
            }

            _context.TPackageCarts.Remove(tPackageCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderlist([FromBody] TPackageCart addPchart)
        {
            if (addPchart == null)
            {
                return BadRequest("order is null.");
            }

            var order = new TPackageCart
            {
                MemberId = addPchart.MemberId,
                PackageId = addPchart.PackageId,
                Qty = addPchart.Qty
            };

            try
            {
                _context.TPackageCarts.Add(order);
                await _context.SaveChangesAsync();
                return Ok(order);
            }
            catch (DbUpdateException ex)
            {
                // 捕获DbUpdateException并输出详细的错误信息
                var sqlException = ex.GetBaseException() as SqlException;
                return StatusCode(500, $"Internal server error: {sqlException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        private bool TPackageCartExists(int id)
        {
            return _context.TPackageCarts.Any(e => e.PackageCartId == id);
        }
    }
}
