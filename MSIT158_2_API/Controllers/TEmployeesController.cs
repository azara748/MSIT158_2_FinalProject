using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public class TEmployeesController : ControllerBase
    {
        private readonly SelectShopContext _context;
        private static string _pp = "";

        public TEmployeesController(SelectShopContext context)
        {
            _context = context;
        }
        //登入員工帳號
        [HttpPost("Employeelogin")]
        public async Task<ActionResult<TMember>> POSTEmployeelogin([FromForm] CLoginViewModel vm)
        {
            TEmployee user = _context.TEmployees.FirstOrDefault(
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
        //確認員工帳號
        [HttpPost("EmployeeCheck")]
        public async Task<ActionResult<TMember>> POSTEmployeeCheck([FromBody] CCheckViewModel vm)
        {
            TEmployee user = _context.TEmployees.FirstOrDefault(t => t.EMail.Equals(vm.txtEmail));

            string json = "";
            if (user != null)
            {
                json = JsonSerializer.Serialize(user);
                _pp = json;
                //HttpContext.Session.SetString(CDictionary.SK_LOGIN_MEMBER, json);
            }
            return Ok(new { message = "確認成功", json });
        }

        //搜尋員工資料
        [HttpPost("EmployeeSearch")]
        public async Task<ActionResult<CEmployePagingDTO>> GetEmployee([FromBody] CESearchDTO searchDTO)
        {
            //根據分類編號搜尋員工資料
            var employees = searchDTO.employeeId == 0 ? _context.TEmployees : _context.TEmployees.Where(s => s.EmployeeId == searchDTO.employeeId);
            //根據關鍵字搜尋員工資料(title、desc)
            if (!string.IsNullOrEmpty(searchDTO.keyword))
                employees = employees.Where(s => s.EmployeeName.Contains(searchDTO.keyword) ||
                s.Address.Contains(searchDTO.keyword) ||
                s.EMail.Contains(searchDTO.keyword));

            //排序
            switch (searchDTO.sortBy)
            {
                case "spotTitle":
                    employees = searchDTO.sortType == "asc" ? employees.OrderBy(s => s.EmployeeName) : employees.OrderByDescending(s => s.EmployeeName);
                    break;
                case "categoryId":
                    employees = searchDTO.sortType == "asc" ? employees.OrderBy(s => s.EMail) : employees.OrderByDescending(s => s.EMail);
                    break;
                default:
                    employees = searchDTO.sortType == "asc" ? employees.OrderBy(s => s.EmployeeId) : employees.OrderByDescending(s => s.EmployeeId);
                    break;
            }

            //總共有多少筆資料
            int totalCount = employees.Count();
            //每頁要顯示幾筆資料
            int pageSize = searchDTO.pageSize;   //searchDTO.pageSize ?? 9;
            //目前第幾頁
            int page = searchDTO.page;

            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            //分頁
            employees = employees.Skip((page - 1) * pageSize).Take(pageSize);


            //包裝要傳給client端的資料
            CEmployePagingDTO employeesPaging = new CEmployePagingDTO();
            employeesPaging.TotalCount = totalCount;
            employeesPaging.TotalPages = totalPages;
            employeesPaging.EmployeesResult = await employees.ToListAsync();

            return employeesPaging;
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
        public async Task<IActionResult> PutTEmployee(int id,[FromForm] CTEmployeeDTO employeeIn, IFormFile avatar)
        {
            if (id != employeeIn.EmployeeId)
            {
                return BadRequest();
            }
            //檔案上傳轉成二進位
            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                avatar.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            TEmployee employeeDb = _context.TEmployees.FirstOrDefault(x => x.EmployeeId == employeeIn.EmployeeId);
            if (employeeDb != null)
            {
                employeeDb.EmployeeName = employeeIn.EmployeeName;
                employeeDb.Address = employeeIn.Address;
                employeeDb.Cellphone = employeeIn.Cellphone;
                employeeDb.Birthday = employeeIn.Birthday;
                employeeDb.EMail = employeeIn.EMail;
                employeeDb.Password = employeeIn.Password;
                employeeDb.OnBoardDate = employeeIn.OnBoardDate;
                employeeDb.DepId = employeeIn.DepId;
                employeeDb.EmployeePhoto = imgByte;
                await _context.SaveChangesAsync();
            }


            //_context.Entry(tEmployee).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!TEmployeeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            return Ok(new { message = "修改成功", employeeDb });
        }

        // POST: api/TEmployees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        public async Task<ActionResult<TEmployee>> PostTEmployee([FromForm] CTEmployeeDTO p, IFormFile avatar)
        {
            //檔案上傳轉成二進位
            byte[] imgByte = null;
            if (avatar != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    avatar.CopyTo(memoryStream);
                    imgByte = memoryStream.ToArray();
                }
            }



            TEmployee e = new TEmployee();
            e.EmployeeId = p.EmployeeId;
            e.EmployeeName = p.EmployeeName;
            e.Cellphone = p.Cellphone;
            e.Address = p.Address;
            e.Birthday = p.Birthday;
            e.Password = p.Password;
            e.EMail = p.EMail;
            e.OnBoardDate = p.OnBoardDate;
            e.DepId = p.DepId;
            e.EmployeePhoto = imgByte;            
            _context.TEmployees.Add(e);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTEmployee", new { id = tEmployee.EmployeeId }, tEmployee);
            return Ok(new { message = "新增成功", e });
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
