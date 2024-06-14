using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;

namespace MSIT158_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly SelectShopContext _db;
        public MembersController(SelectShopContext db) 
        {
            _db = db;
        }

        [HttpPost]
        public async Task<ActionResult<TMember>> PostTMember([FromBody] CMemberDTO p)
        {

            TMember m = new TMember();
            m.MemberName = p.MemberName;
            _db.TMembers.Add(m);
            await _db.SaveChangesAsync();

            return Ok(new { message = "新增成功", m });
        }

        //[HttpPost]
        //public IActionResult PostTMember(string p)
        //{
        //    TMember m = new TMember();
        //    //m.MemberName = p.MemberName;
        //    //_db.TMembers.Add(m);
        //    //await _db.SaveChangesAsync();
        //    return Ok(new { message = "新增成功", m });
        //}

    }
}
