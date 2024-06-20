using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;

namespace MSIT158_2_API.Controllers.Back
{
    [Route("api/[controller]")]
    [ApiController]
    public class TLabelsController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public TLabelsController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/TLabels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TLabel>>> GetTLabels()
        {
            return await _context.TLabels.ToListAsync();
        }

        // GET: api/TLabels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TLabel>> GetTLabel(int id)
        {
            var tLabel = await _context.TLabels.FindAsync(id);

            if (tLabel == null)
            {
                return NotFound();
            }

            return tLabel;
        }

        // PUT: api/TLabels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTLabel(int id, TLabel tLabel)
        {
            if (id != tLabel.LabelId)
            {
                return BadRequest();
            }

            _context.Entry(tLabel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TLabelExists(id))
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

        // POST: api/TLabels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TLabel>> PostTLabel(TLabel tLabel)
        {
            _context.TLabels.Add(tLabel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTLabel", new { id = tLabel.LabelId }, tLabel);
        }
        [HttpPost("getLabelName")]
        public async Task<ActionResult<object>> getLabelName(int labid)
        {
            IQueryable<object> query;

            if (labid == 0)
            {
                query = _context.TLabels
                        .Select(c => new { c.LabelId, c.LabelName });
            }
            else
            {
                query = _context.TLabels
                        .Where(c => c.LabelId == labid)
                        .Select(c => new { c.LabelId, c.LabelName });
            }
            var result = await query.ToListAsync();

            if (!result.Any())
            {
                return NotFound();
            }

            return result;
        }

        // DELETE: api/TLabels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTLabel(int id)
        {
            var tLabel = await _context.TLabels.FindAsync(id);
            if (tLabel == null)
            {
                return NotFound();
            }

            _context.TLabels.Remove(tLabel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TLabelExists(int id)
        {
            return _context.TLabels.Any(e => e.LabelId == id);
        }
    }
}
