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
using MSIT158_2_API.Models.DTO.Member;
using MSIT158_2_API.ViewModel;

namespace MSIT158_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TMembersController : ControllerBase
    {
        private readonly SelectShopContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private static string _pp = "";
        // 建構子注入資料庫上下文
        public TMembersController(SelectShopContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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
            }
            return Ok(new { message = "登入成功", json });
        }
        //確認會員帳號
        [HttpPost("MemberCheck")]
        public async Task<ActionResult<TMember>> POSTMemberCheck([FromBody] CCheckViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));
            if (user == null)
                return BadRequest(new { message = "沒有電子郵件，將無法寄信修改密碼" });
            return Ok(new { message = "確認成功", user });
        }
        //忘記密碼1(檢查帳戶)
        [HttpPost("MemberForgetPassword")]
        public async Task<ActionResult<TMember>> POSTMemberForgetPassword([FromForm] CCheckViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));
            if (user == null)
                return BadRequest(new { message = "沒有電子郵件，將無法寄信修改密碼" });
            return Ok(new { message = "確認成功", user });
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
        //忘記密碼A(修改密碼)
        [HttpPost("SenderForgetPasswordEmail")]
        public async Task<ActionResult<TMember>> POSTSenderForgetPasswordEmail([FromForm] CCheckViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));

            //如果沒有密碼，將無法寄信修改密碼
            if (string.IsNullOrEmpty(user.Password))
                return BadRequest(new { message = "沒有密碼，將無法寄信修改密碼" });

            string receive = user.EMail;
            string subject = $"{user.MemberName}用戶重新設定密碼";
            string messages = $"<h1>修改{user.MemberName}的密碼</h1>";
            //messages += "<form method=\"post\" action=\"https://localhost:7160/api/TMembers/SenderEditPassword\" id=\"userForm\" enctype=\"multipart/form-data\">";
            //messages += "<div class=\"mb-3\">";
            //messages += "<label for=\"InputEmail\" class=\"form-label\">電子郵件：</label>";
            //messages += $"<input type=\"email\" class=\"form-control\" id=\"InputEmail\" name=\"txtEmail\" value=\"{user.EMail}\">";
            //messages += "</div>";
            //messages += "<div class=\"mb-3\">";
            //messages += "<label for=\"InputPassword\" class=\"form-label\">新密碼：</label>";
            //messages += "<input type=\"text\" class=\"form-control\" id=\"InputPassword\" name=\"txtPassword\">";
            //messages += "</div>";
            //messages += "<button type=\"submit\" class=\"btn btn-primary\" id=\"buttonSubmit\">修改新密碼並送出</button>";
            //messages += "</form>";
            //messages += "<p>請點擊以下連結回到登入頁面:</p>";
            //messages += "<a href='https://localhost:7066/Home/Login'>回到登入頁面</a>點擊這裡";
            messages += "<a href='https://localhost:7066/Home/MemberForgetPasswordB'>修改密碼</a>點擊這裡";
            //messages += "<script>console.log(\"test1\");</script>";
            new CEmailSender().getEmail(receive, subject, messages);

            return Ok(new { message = "郵件已成功發送" });
        }
        //忘記密碼B(修改新密碼)
        [HttpPost("SenderEditPassword")]
        public async Task<ActionResult<TMember>> SenderEditPassword([FromForm] CLoginViewModel vm)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));
            if (user == null)
                return BadRequest(new { message = "沒有電子郵件，將修改密碼" });
            // 從資料庫中獲取用戶的鹽和雜湊後的密碼
            string salt = user.Salt;
            string Passwordsalted = vm.txtPassword + salt;
            //密碼加密，使用 SHA256 演算法
            vm.txtPassword = GetSha256Hash(Passwordsalted);
            user.Password = vm.txtPassword;
            await _context.SaveChangesAsync();
            //string json = JsonSerializer.Serialize(user);

            return Ok(new { message = "密碼修改成功", user });
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
            var membersjoin = await _context.TMembers
                .Include(m => m.Vip)
                .ToListAsync();
            string uploadPath = null;
            List<CmemberDetailDTO> cmemberDetailDTOs = new List<CmemberDetailDTO>();
            foreach (var member in membersjoin)
            {
                if (member.MemberStatus == 1)
                {
                    if (string.IsNullOrEmpty(member.Cellphone))
                        member.Cellphone = "";
                    if (string.IsNullOrEmpty(member.Address))
                        member.Address = "";
                    if (string.IsNullOrEmpty(member.Sex))
                        member.Sex = "";
                    if (string.IsNullOrEmpty(member.Password))
                        member.Password = "";
                    if (string.IsNullOrEmpty(member.Salt))
                        member.Salt = "";
                    if (string.IsNullOrEmpty(member.EMail))
                        member.EMail = "";
                    if (member.Birthday == null)
                        member.Birthday = new DateOnly(2000, 01, 01);

                    if (string.IsNullOrEmpty(member.MemberPhoto))
                        member.MemberPhoto = "default.jpg"; // 預設圖片名稱

                    //照片實際路徑
                    uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", member.MemberPhoto);
                    if (!System.IO.File.Exists(uploadPath))
                        uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", "default.jpg"); // 預設圖片路徑

                    //使用 System.IO.File.ReadAllBytes(filePath) 讀取文件內容並將其轉換為字節數組。
                    var imageBytes = System.IO.File.ReadAllBytes(uploadPath);
                    //使用 Convert.ToBase64String(imageBytes) 將字節數組轉換為 Base64 字符串。
                    var base64String = Convert.ToBase64String(imageBytes);
                    //使用 data:{contentType};base64,{base64String} 格式生成 Base64 數據 URI，其中 {contentType} 是文件的 MIME 類型，
                    var contentType = new CGetContentType().GetContentType(uploadPath);
                    //{base64String} 是 Base64 編碼後的文件內容。
                    var base64Image = $"data:{contentType};base64,{base64String}";
                    CmemberDetailDTO cmemberDetailDTO = new CmemberDetailDTO()
                    {
                        Id = member.MemberId,
                        MemberName = member.MemberName,
                        Cellphone = member.Cellphone,
                        Address = member.Address,
                        Birthday = member.Birthday,
                        Sex = member.Sex,
                        Password = member.Password,
                        Salt = member.Salt,
                        EMail = member.EMail,
                        Points = member.Points,
                        Vip = member.Vip?.Vipname,
                        Vipphoto = member.Vip?.Vipphoto,
                        MemberPhoto = base64Image,
                        Wallet = member.Wallet
                    };
                    cmemberDetailDTOs.Add(cmemberDetailDTO);
                }
            }


            //根據分類編號搜尋會員資料
            var members = searchDTO.memberId == 0 ? cmemberDetailDTOs : cmemberDetailDTOs.Where(s => s.Id == searchDTO.memberId);
            //根據關鍵字搜尋會員資料(title)
            if (!string.IsNullOrEmpty(searchDTO.keyword))
                members = members.Where(s => s.MemberName != null && s.MemberName.Contains(searchDTO.keyword) ||
                    s.Address != null && s.Address.Contains(searchDTO.keyword) ||
                    s.EMail != null && s.EMail.Contains(searchDTO.keyword));

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
                    members = searchDTO.sortType == "asc" ? members.OrderBy(s => s.Id) : members.OrderByDescending(s => s.Id);
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
            membersPaging.MembersResult = members.ToList();

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
            m.MemberStatus = 1;
            m.Vipid = 1;
            _context.TMembers.Add(m);
            await _context.SaveChangesAsync();
            //註冊會員時，發送Email
            string receive = p.EMail;
            string subject = "Gifty 用戶註冊驗證";
            string messages = $"<h1>{p.MemberName}歡迎註冊 Gifty</h1>";
            messages += "<p>請點擊以下連結驗證您的帳號:</p>";
            messages += "<a href='https://localhost:7066/Home/Login'>點擊這裡</a>進行驗證";
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
            TMember user = _context.TMembers.FirstOrDefault(m => m.MemberId == id);

            if (string.IsNullOrEmpty(user.MemberPhoto))
                user.MemberPhoto = "default.jpg"; // 預設圖片名稱

            //照片實際路徑
            string uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", user.MemberPhoto);
            if (!System.IO.File.Exists(uploadPath))
                uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", "default.jpg"); // 預設圖片路徑

            //使用 System.IO.File.ReadAllBytes(filePath) 讀取文件內容並將其轉換為字節數組。
            var imageBytes = System.IO.File.ReadAllBytes(uploadPath);
            //使用 Convert.ToBase64String(imageBytes) 將字節數組轉換為 Base64 字符串。
            var base64String = Convert.ToBase64String(imageBytes);
            //使用 data:{contentType};base64,{base64String} 格式生成 Base64 數據 URI，其中 {contentType} 是文件的 MIME 類型，
            var contentType = new CGetContentType().GetContentType(uploadPath);
            //{base64String} 是 Base64 編碼後的文件內容。
            var base64Image = $"data:{contentType};base64,{base64String}";

            //var tMember = await _context.TMembers.FindAsync(id);

            //if (tMember == null)
            //{
            //    return NotFound();
            //}

            return Ok(new { base64Image });
        }

        // PUT: api/TMembers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTMember(int id,[FromForm] CMemberEditDTO memberIn, IFormFile avatar)
        {
            if (id != memberIn.MemberId)
            {
                return BadRequest();
            }
            
            // 從資料庫中獲取用戶的鹽和雜湊後的密碼
            string salt = memberIn.Salt;
            string Passwordsalted = memberIn.Password + salt;
            //密碼加密，使用 SHA256 演算法
            memberIn.Password = GetSha256Hash(Passwordsalted);

            //照片實際路徑
            string uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", avatar.FileName);
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                avatar.CopyTo(fileStream);
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
                memberDb.MemberPhoto = avatar.FileName;
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
        [HttpGet("MemberDelete{id}")]
        public async Task<ActionResult<TMember>> POSTMemberDelete(int id)
        {
            TMember user = _context.TMembers.FirstOrDefault(t => t.MemberId.Equals(id));
            if (user == null)
                return BadRequest(new { message = "沒有會員資料，將無法刪除會員" });
            user.MemberStatus = 2;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("ReceiveCashFlow")]
        public async Task<ActionResult<TMember>> ReceiveCashFlow([FromForm] Dictionary<string,string> ecPayData)
        {           
            var element6 = ecPayData.ElementAt(6); // 取出第6個元素 (索引從0開始)
            var element7 = ecPayData.ElementAt(7); 
            var element10 = ecPayData.ElementAt(10); // 取出第10個元素 (索引從0開始)
            string value6 = element6.Value; // [PaymentDate, 2024/06/20 15:10:25]
            string value7 = element7.Value; // [PaymentType, Credit_CreditCard]
            string value10 = element10.Value; // [RtnMsg, 交易成功]
            string value5 = ecPayData.ElementAt(5).Value; // [MerchantTradeNo, 603e85fcf33642d4b1ca]
            var o = _context.TOrders.FirstOrDefault(x=>x.MerchantTradeNo == value5);
            if (o == null)
                return BadRequest(new { message = "綠界訂單編號不一致" });
            o.PaymentType = value7;
            await _context.SaveChangesAsync();

            return Ok(new { message = "確認成功", value7 });
        }

        [HttpGet("MemberSumAmount{id}")]
        public async Task<ActionResult<TMember>> POSTMemberSumAmount(int id)
        {
            var MemberAmount = await _context.TOrders
                .Include(m => m.TPurchases)
                .ThenInclude(p => p.Product)
                .Include(m => m.TPackageWayDetails)
                .ThenInclude(k => k.Package)
                .Include(m => m.Member)
                .Where(m => m.MemberId == id)
                .ToListAsync();

            decimal total = 0;
            decimal totalA = 0;
            decimal totalB = 0;
            foreach (var order in MemberAmount)
            {
                foreach (var purchase in order.TPurchases)
                {
                    if (purchase.Product != null && purchase.Qty.HasValue && purchase.Product.UnitPrice.HasValue)
                    {
                        decimal amount = purchase.Qty.Value;
                        decimal price = purchase.Product.UnitPrice.Value;

                        totalA += amount * price;
                    }
                }
                foreach (var packageway in order.TPackageWayDetails)
                {
                    if (packageway.Package != null && packageway.PackQty.HasValue && packageway.Package.Price.HasValue)
                    {
                        decimal packageAmount = packageway.PackQty.Value;
                        decimal packagePrice = packageway.Package.Price.Value;

                        totalB += packageAmount * packagePrice;
                    }
                }
            }
            total = totalA + totalB;
            return Ok(new { message = "查詢成功", total });
        }
        private bool TMemberExists(int id)
        {
            return _context.TMembers.Any(e => e.MemberId == id);
        }
    }
}
