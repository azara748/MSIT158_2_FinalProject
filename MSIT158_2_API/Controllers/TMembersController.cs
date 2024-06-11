using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT158_2_API.Models;
using MSIT158_2_API.Models.DTO;
using MSIT158_2_API.ViewModel;

namespace MSIT158_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TMembersController : ControllerBase
    {
        private readonly SelectShopContext _context;
        private static string _pp = "";
        // 建構子注入資料庫上下文
        public TMembersController(SelectShopContext context)
        {
            _context = context;
        }
        //登入會員帳號
        [HttpPost("Memberlogin")]
        public async Task<ActionResult<TMember>> POSTMemberLogin([FromBody] CLoginViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(
                t => t.EMail.Equals(vm.txtEmail) && t.Password.Equals(vm.txtPassword));

            string json = "";
            if (user != null && user.Password.Equals(vm.txtPassword))
            {
                json = JsonSerializer.Serialize(user);
                _pp = json;
                //HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);
            }
            return Ok(new { message = "登入成功", json });
        }
        //確認會員帳號
        [HttpPost("MemberCheck")]
        public async Task<ActionResult<TMember>> POSTMemberCheck([FromBody] CCheckViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));

            string json = "";
            if (user != null)
            {
                json = JsonSerializer.Serialize(user);
                _pp = json;
                //HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);
            }
            return Ok(new { message = "確認成功", json });
        }
        //忘記密碼
        [HttpPost("MemberForgetPassword")]
        public async Task<ActionResult<TMember>> POSTMemberForgetPassword([FromBody] CCheckViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));

            string json = "";
            if (user != null)
            {
                json = JsonSerializer.Serialize(user);

            }
            return Ok(new { message = "確認成功", json });
        }
        //忘記密碼2(修改密碼)
        [HttpPost("MemberEditPassword")]
        public async Task<ActionResult<TMember>> POSTMemberEditPassword([FromBody] CLoginViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));

            string json = "";
            if (user != null)
            {
                user.Password = vm.txtPassword;
                await _context.SaveChangesAsync();
                json = JsonSerializer.Serialize(user);

            }
            return Ok(new { message = "密碼修改成功", json });
        }



        //搜尋會員資料
        [HttpPost("MemberSearch")]
        public async Task<ActionResult<CMembersPagingDTO>> GetMembers([FromBody] CSearchDTO searchDTO)
        {
            //根據分類編號搜尋會員資料
            var members = searchDTO.memberId == 0 ? _context.TMembers : _context.TMembers.Where(s => s.MemberId == searchDTO.memberId);
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
        //檢查帳號是否存在
        [HttpPost("CheckAccount")]
        //public async Task<ActionResult<TMember>> CheckAccount([FromBody] string email, [FromBody] string password)
        //{
        //    // 從資料庫中獲取用戶的鹽和雜湊後的密碼
        //    TMember used = _context.TMembers.FirstOrDefault(x => x.EMail == email);
        //    string salt = used.Salt;
        //    string Passwordsalted = password + salt;
        //    //密碼加密，使用 SHA256 演算法
        //    password = GetSha256Hash(Passwordsalted);
        //    var is_mail = _context.TMembers.Any(m => m.EMail == email);
        //    var is_password = _context.TMembers.Any(m => m.Password == password);
        //    var Memail = _context.TMembers.Where(x => x.EMail == email);
        //    var Mpassword = _context.TMembers.Where(x => x.Password == password);
        //    var str = "帳號可使用";
        //    var str2 = "信箱可使用";
        //    string json = "";
            

        //    //str2 = "信箱已存在";
        //    //var str3 = str + "<br />" + str2;

        //    if (is_password)
        //        Mpassword.ToString();
        //    //str = "密碼已存在";

        //    var strA = "<br />" + Mpassword.ToString();
        //    var str3 = str2 + "<br />" + str;
        //    //======================================================
        //    TMember user = _context.TMembers.FirstOrDefault(
        //        t => t.EMail.Equals(email) && t.Password.Equals(password));

        //    if (user != null && user.Password.Equals(password))
        //    {
        //        json = JsonSerializer.Serialize(user);
        //        //HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);

        //        pp = json;
        //    }
        //    //=====================================================


        //    return Content(json, "text/plain", System.Text.Encoding.UTF8);
        //}

        //登入加密&確認密碼
        [HttpPost("CheckPassword")]
        public async Task<ActionResult<TMember>> PostTMemberCheckPassword([FromBody] CMemberEditDTO p, string repassword)
        {

            var is_name = _context.TMembers.Any(x => x.MemberName == p.MemberName);
            var is_mail = _context.TMembers.Any(x => x.EMail == p.EMail);
            var is_password = _context.TMembers.Any(x => x.Password == p.Password);
            string str = "密碼有誤";
            string str1 = "帳號可使用";
            string str2 = "信箱可使用";
            string str3 = "密碼可使用";

            if (is_name)
                str1 = "帳號已存在";
            if (is_mail)
                str2 = "信箱已存在";
            if (string.IsNullOrEmpty(p.MemberName))
                p.MemberName = "guset";

            //檔案上傳轉成二進位
            byte[] imgByte = null;
            //if (p.Password == repassword)
            if (false)
            {
                str = "密碼可使用";
                if (is_password)
                    str3 = "密碼已存在";

                //檔案上傳轉成二進位
                //if (p.avatar != null)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        p.avatar.CopyTo(memoryStream);
                //        imgByte = memoryStream.ToArray();
                //    }
                //}

                // 產生一個隨機鹽
                string salt = CreateSalt();
                string Passwordsalted = p.Password + salt;
                //密碼加密，使用 SHA256 演算法
                p.Password = GetSha256Hash(Passwordsalted);

                //信箱不重複，才可以寫進資料庫
                if (!is_mail)
                {
                    //p.MemberPhoto = imgByte;
                    p.Salt = salt;
                    //_context.TMembers.Add(p);
                    //_context.SaveChanges();
                }
            }


            if (!string.IsNullOrEmpty(p.EMail) && !string.IsNullOrEmpty(p.Password))
                str = str + "<hr />" + str1 + "<hr />" + str2 + "<hr />" + str3 + "<br />" + "上傳成功";
            else
                str = "信箱&密碼 都要輸入";
            return Content(str.ToString(), "text/html", System.Text.Encoding.UTF8);
        }
        //密碼，產生一個隨機鹽
        private string CreateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        // 使用 SHA256 演算法對密碼進行加密處理
        private string GetSha256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // 將 byte 陣列轉換成 16 進位字串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }




        // GET: api/TMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TMember>>> GetTMembers()
        {
            return await _context.TMembers.ToListAsync();
        }

        // GET: api/TMembers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TMember>> GetTMember(int id)
        {
            var tMember = await _context.TMembers.FindAsync(id);

            if (tMember == null)
            {
                return NotFound();
            }

            return tMember;
        }

        // PUT: api/TMembers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTMember(int id, CMemberEditDTO memberIn)
        {
            if (id != memberIn.MemberId)
            {
                return BadRequest();
            }
            TMember memberDb = _context.TMembers.FirstOrDefault(x => x.MemberId == memberIn.MemberId);
            if (memberDb != null)
            {
                memberDb.MemberName = memberIn.MemberName;
                memberDb.Address = memberIn.Address;
                memberDb.Cellphone = memberIn.Cellphone;
                memberDb.Birthday = memberIn.Birthday;
                memberDb.Sex = memberIn.Sex;
                memberDb.Password = memberIn.Password;
                memberDb.Points = memberIn.Points;
                memberDb.Vipid = memberIn.Vipid;
                memberDb.Wallet = memberIn.Wallet;
                memberDb.Salt = memberIn.Salt;
                memberDb.EMail = memberIn.EMail;
                memberDb.MemberPhoto = memberIn.MemberPhoto;
                await _context.SaveChangesAsync();
            }


            //_context.Entry(tMember).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!TMemberExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            return Ok(new { message = "修改成功", memberDb });
        }

        // POST: api/TMembers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Route("Create")]
        [HttpPost("Create")]
        public async Task<ActionResult<TMember>> PostTMember([FromBody] CMemberEditDTO p)
        {
            TMember m = new TMember();
            m.MemberName = p.MemberName;
            m.Cellphone = p.Cellphone;
            m.EMail = p.EMail;
            m.Address = p.Address;
            m.Sex = p.Sex;
            m.Password = p.Password;
            m.MemberPhoto = p.MemberPhoto;
            _context.TMembers.Add(m);
            await _context.SaveChangesAsync();

            //CreatedAtAction("GetTMember", new { id = tMember.MemberId }, tMember);
            return Ok(new { message = "新增成功", m });
        }

        // DELETE: api/TMembers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTMember(int id)
        {
            var tMember = await _context.TMembers.FindAsync(id);
            if (tMember == null)
            {
                return NotFound();
            }

            _context.TMembers.Remove(tMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TMemberExists(int id)
        {
            return _context.TMembers.Any(e => e.MemberId == id);
        }
    }
}
