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
        public async Task<ActionResult<TMember>> POSTMemberLogin([FromForm] CLoginViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(
                t => t.EMail.Equals(vm.txtEmail) && t.Password.Equals(vm.txtPassword));

            string json = "";
            if (user != null && user.Password.Equals(vm.txtPassword))
            {
                json = JsonSerializer.Serialize(user);
                //_pp = json;
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
        //忘記密碼1(檢查帳戶)
        [HttpPost("MemberForgetPassword")]
        public async Task<ActionResult<TMember>> POSTMemberForgetPassword([FromForm] CCheckViewModel vm)
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
        public async Task<ActionResult<TMember>> POSTMemberEditPassword([FromForm] CLoginViewModel vm)
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
        //Google,Facebook 登入，新增資料
        [HttpPost("OauthCreate")]
        public async Task<ActionResult<TMember>> POSTMemberOauthCreate([FromBody] COauthLoginViewModel vm)
        {
            TMember is_email = _context.TMembers.FirstOrDefault(x => x.EMail == vm.txtOauthEmail);
            if (is_email != null)
                return Ok(new { message = "該會員已存在", m = is_email });


            TMember m = new TMember();
            m.MemberName = vm.txtOauthName;
            m.EMail = vm.txtOauthEmail;


            _context.TMembers.Add(m);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "新增成功", m });
        }
        //Google,Facebook 登入，尋找資料
        [HttpPost("OauthSearchMember")]
        public async Task<ActionResult<TMember>> POSTMemberOauthSearchMember([FromBody] COauthLoginViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(
                t => t.EMail.Equals(vm.txtOauthEmail) && t.MemberName.Equals(vm.txtOauthName));

            string json = "";
            if (user != null && user.EMail.Equals(vm.txtOauthEmail))
            {
                json = JsonSerializer.Serialize(user);
                _pp = json;
                //HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);
            }
            return Ok(new { message = "登入成功", user });
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
        public async Task<ActionResult<TMember>> CheckAccount([FromForm] string email, [FromForm] string password)
        {
            // 從資料庫中獲取用戶的鹽和雜湊後的密碼
            TMember used = _context.TMembers.FirstOrDefault(x => x.EMail == email);
            string salt = used.Salt;
            string Passwordsalted = password + salt;
            //密碼加密，使用 SHA256 演算法
            password = GetSha256Hash(Passwordsalted);
            var is_mail = _context.TMembers.Any(m => m.EMail == email);
            var is_password = _context.TMembers.Any(m => m.Password == password);
            var Memail = _context.TMembers.Where(x => x.EMail == email);
            var Mpassword = _context.TMembers.Where(x => x.Password == password);
            var str = "帳號可使用";
            var str2 = "信箱可使用";
            string json = "";


            //str2 = "信箱已存在";
            //var str3 = str + "<br />" + str2;

            if (is_password)
                Mpassword.ToString();
            //str = "密碼已存在";

            var strA = "<br />" + Mpassword.ToString();
            var str3 = str2 + "<br />" + str;
            //======================================================
            TMember user = _context.TMembers.FirstOrDefault(
                t => t.EMail.Equals(email) && t.Password.Equals(password));

            if (user != null && user.Password.Equals(password))
            {
                json = JsonSerializer.Serialize(user);
                //HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);

                _pp = json;
            }
            //=====================================================


            return Content(json, "text/plain", System.Text.Encoding.UTF8);
        }

        //新增會員&確認密碼
        [HttpPost("CreateCheckPassword")]
        public async Task<ActionResult<TMember>> PostTMemberCreateCheckPassword([FromForm] CMemberEditDTO p, [FromForm] string repassword)
        {
            var is_mail = _context.TMembers.Any(x => x.EMail == p.EMail);
            var is_password = _context.TMembers.Any(x => x.Password == p.Password);
            // 檢查信箱是否已經存在(信箱不重複，才可以寫進資料庫)
            //if (is_mail)
            //    return BadRequest(new { message = "信箱已存在" });
            // 檢查密碼和重複輸入的密碼是否一致
            if (p.Password != repassword)
                return BadRequest(new { message = "密碼不一致" });
            // 如果會員名稱為空，設置默認值            
            if (string.IsNullOrEmpty(p.MemberName))
                p.MemberName = "guset";
            //檔案上傳轉成二進位
            byte[] imgByte = null;
            IFormFile avatar = null;
            //檔案上傳轉成二進位
            if (avatar != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    avatar.CopyTo(memoryStream);
                    imgByte = memoryStream.ToArray();
                }
            }

            // 產生一個隨機鹽
            string salt = CreateSalt();
            string Passwordsalted = p.Password + salt;
            //密碼加密，使用 SHA256 演算法
            p.Password = GetSha256Hash(Passwordsalted);

            TMember m = new TMember();
            //p.MemberPhoto = imgByte;
            m.MemberName = p.MemberName;
            m.Address = p.Address;
            m.Cellphone = p.Cellphone;
            m.Sex = p.Sex;
            m.Password = p.Password;
            m.Salt = salt;
            m.Points = p.Points;
            m.EMail = p.EMail;
            m.MemberPhoto = p.MemberPhoto;
            _context.TMembers.Add(m);
            await _context.SaveChangesAsync();
            //註冊會員時，發送Email
            string receive = p.EMail;
            string subject = "*** 用戶註冊驗證";
            string messages = $"<h1>{p.MemberName}歡迎註冊***</h1>";
            messages += "<p>請點擊以下連結驗證您的帳號:</p>";
            messages += "<a>點擊這裡</a>進行驗證";
            new CEmailSender().getEmail(receive, subject, messages);

            return Ok(new { message = "新增成功", m });
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
        public async Task<ActionResult<TMember>> PostTMember([FromForm] CMemberEditDTO p)
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
