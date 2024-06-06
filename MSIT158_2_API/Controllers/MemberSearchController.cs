using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;

namespace MSIT158_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberSearchController : ControllerBase
    {
        private readonly SelectShopContext _db;
        public MemberSearchController(SelectShopContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<ActionResult<CMembersPagingDTO>> GetMembers([FromBody] CSearchDTO searchDTO)
        {
            //根據分類編號搜尋會員資料
            var members = searchDTO.memberId == 0 ? _db.TMembers : _db.TMembers.Where(s => s.MemberId == searchDTO.memberId);
            //根據關鍵字搜尋會員資料(title、desc)
            if (!string.IsNullOrEmpty(searchDTO.keyword))
                members = members.Where(s => s.MemberName.Contains(searchDTO.keyword) ||
                s.Address.Contains(searchDTO.keyword) ||
                s.EMail.Contains(searchDTO.keyword));

            //排序
            switch (searchDTO.sortBy)
            {
                case "spotTitle":
                    members = searchDTO.sortType == "asc" ? members.OrderBy(s => s.MemberName) : members.OrderByDescending(s => s.MemberName);
                    break;
                case "categoryId":
                    members = searchDTO.sortType == "asc" ? members.OrderBy(s => s.EMail) : members.OrderByDescending(s => s.EMail);
                    break;
                default:
                    members = searchDTO.sortType == "asc" ? members.OrderBy(s => s.MemberId) : members.OrderByDescending(s => s.MemberId);
                    break;
            }

            //總共有多少筆資料
            int totalCount = members.Count();
            //每頁要顯示幾筆資料
            int pageSize = searchDTO.pageSize;   //searchDTO.pageSize ?? 9;
            //目前第幾頁
            int page = searchDTO.page;

            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            //分頁
            members = members.Skip((page - 1) * pageSize).Take(pageSize);


            //包裝要傳給client端的資料
            CMembersPagingDTO membersPaging = new CMembersPagingDTO();
            membersPaging.TotalCount = totalCount;
            membersPaging.TotalPages = totalPages;
            membersPaging.MembersResult = await members.ToListAsync();

            return membersPaging;
        }
    }
}
