using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_FinalProject.Models;
using MSIT158_2_FinalProject.Models.DTO;
using NuGet.Protocol;

namespace MSIT158_2_FinalProject.Controllers.Front
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

        //存進去: 如果member喜歡產品->給出產品id 如果表裡面有該產品id =>狀態改變 0沒有 1有
        //回傳

        [HttpPost]
        public async Task<ActionResult<ShowMemberLikeDTO>> GetMemberLike(int id)
        {
            var query = _context.TMemberLikes
                        .Include(x => x.Memeber)
                        .Include(x => x.Product)
                        .Where(m => m.MemeberId == id)
                        .Select(m => new ShowMemberLikeDTO
                        {
                            MemeberId = m.MemeberId,
                            LikeId = m.LikeId,
                            ProductId = m.ProductId,
                            ProductName = m.Product.ProductName,
                            Productphoto = m.Product.ProductPhoto,
                            UnitPrice = m.Product.UnitPrice
                        }).ToList();

            var ans = new MemberLikeListDTO
            {
                count = query.Count,
                showMemberLikeDTOs = query
            };

            return Ok(ans);

        }
        [HttpPost("addlike")]
        public async Task<ActionResult<ShowMemberLikeDTO>> AddMemberLike(int mid, int pid)
        {
            //回傳的東西: 會員id 商品id
            var likeID = _context.TMemberLikes.Where(m => m.MemeberId == mid && m.ProductId == pid)
                         .Select(m => m.LikeId).FirstOrDefault();

            if (likeID != 0)
            {
                var likeToDelete = _context.TMemberLikes.FirstOrDefault(m => m.LikeId == likeID);
                _context.TMemberLikes.Remove(likeToDelete);
                await _context.SaveChangesAsync();
                return Ok(new MemberLikeDTO { IsAdded = false, Message = "已將商品移除願望清單" });
            }
            else
            {
                var newMemberLike = new TMemberLike
                {
                    MemeberId = mid,
                    ProductId = pid,
                };
                _context.TMemberLikes.Add(newMemberLike);
                await _context.SaveChangesAsync();
                return Ok(new MemberLikeDTO { IsAdded = true, Message = "成功加入願望清單" });


            }

            return BadRequest("失敗");
        }
        [HttpPost("dislikeall")]
        public async Task<ActionResult<ShowMemberLikeDTO>> DisMemberLike(int mid)
        {
            //回傳的東西: 會員id 商品id
            var dislikeall = _context.TMemberLikes.Where(m => m.MemeberId == mid)
                            .ToList();

            if (dislikeall.Any())
            {
                // 删除所有查询到的记录
                _context.TMemberLikes.RemoveRange(dislikeall);
                await _context.SaveChangesAsync();
                return Ok("已成功刪除所有願望清單!");
            }
            else
            {
                return Ok("沒有願望商品!快去逛逛其他商品吧!!");
            }

            return BadRequest("失敗");
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
