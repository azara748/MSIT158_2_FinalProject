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
    public class TMemberLikesController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public TMemberLikesController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/TMemberLikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TMemberLike>>> GetTMemberLikes()
        {
            return await _context.TMemberLikes.ToListAsync();
        }

        // GET: api/TMemberLikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TMemberLike>> GetTMemberLike(int id)
        {
            var tMemberLike = await _context.TMemberLikes.FindAsync(id);

            if (tMemberLike == null)
            {
                return NotFound();
            }

            return tMemberLike;
        }

        // PUT: api/TMemberLikes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTMemberLike(int id, TMemberLike tMemberLike)
        {
            if (id != tMemberLike.LikeId)
            {
                return BadRequest();
            }

            _context.Entry(tMemberLike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TMemberLikeExists(id))
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

        // POST: api/TMemberLikes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<TMemberLike>> PostTMemberLike(TMemberLike tMemberLike)
        //{
        //    _context.TMemberLikes.Add(tMemberLike);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTMemberLike", new { id = tMemberLike.LikeId }, tMemberLike);
        //}
        [HttpPost]
        public async Task<ActionResult<TMemberLike>> GetMemberLike()
        { 
            var query = _context.TMemberLikes.Include(t => t.LikeId)

		}

			// DELETE: api/TMemberLikes/5
			[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTMemberLike(int id)
        {
            var tMemberLike = await _context.TMemberLikes.FindAsync(id);
            if (tMemberLike == null)
            {
                return NotFound();
            }

            _context.TMemberLikes.Remove(tMemberLike);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TMemberLikeExists(int id)
        {
            return _context.TMemberLikes.Any(e => e.LikeId == id);
        }
    }
}
