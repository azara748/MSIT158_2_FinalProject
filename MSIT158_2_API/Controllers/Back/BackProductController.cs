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
    public class BackProductController : ControllerBase
    {
        private readonly SelectShopContext _context;

        public BackProductController(SelectShopContext context)
        {
            _context = context;
        }

        // GET: api/BackProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TProduct>>> GetTProducts()
        {
            return await _context.TProducts.ToListAsync();
        }

        // GET: api/BackProduct/5
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

        // PUT: api/BackProduct/5
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

        // POST: api/BackProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<TProduct>> PostTProduct(TProduct tProduct)
        //{
        //    _context.TProducts.Add(tProduct);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTProduct", new { id = tProduct.ProductId }, tProduct);
        //}


        //後台CRUD功能
        //新增 接受產品DTO之後傳入 ??
        //修改 用id讀出資料回傳 API 接受id->回傳資料
        //查詢 接受條件->回傳資料
        //刪除 找出id 刪掉 打勾都刪掉



        [HttpPost]
        public async Task<ActionResult<TProduct>> PostTProduct(TProduct tProduct)
        {
            _context.TProducts.Add(tProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTProduct", new { id = tProduct.ProductId }, tProduct);
        }

        // DELETE: api/BackProduct/5
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
    }
}
