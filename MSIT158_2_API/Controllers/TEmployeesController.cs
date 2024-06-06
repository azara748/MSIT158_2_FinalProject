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
    public class TEmployeesController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public TEmployeesController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/TEmployees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEmployee>>> GetTEmployees()
        {
            return await _context.TEmployees.ToListAsync();
        }

        // GET: api/TEmployees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEmployee>> GetTEmployee(int id)
        {
            var tEmployee = await _context.TEmployees.FindAsync(id);

            if (tEmployee == null)
            {
                return NotFound();
            }

            return tEmployee;
        }

        // PUT: api/TEmployees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTEmployee(int id, TEmployee tEmployee)
        {
            if (id != tEmployee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(tEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TEmployeeExists(id))
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

        // POST: api/TEmployees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TEmployee>> PostTEmployee(TEmployee tEmployee)
        {
            _context.TEmployees.Add(tEmployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTEmployee", new { id = tEmployee.EmployeeId }, tEmployee);
        }

        // DELETE: api/TEmployees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTEmployee(int id)
        {
            var tEmployee = await _context.TEmployees.FindAsync(id);
            if (tEmployee == null)
            {
                return NotFound();
            }

            _context.TEmployees.Remove(tEmployee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TEmployeeExists(int id)
        {
            return _context.TEmployees.Any(e => e.EmployeeId == id);
        }
    }
}
