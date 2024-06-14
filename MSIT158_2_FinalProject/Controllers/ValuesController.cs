using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MSIT158_2_FinalProject.Controllers
{
    [Authorize]
    public class ValuesController : Controller
    {
        public IActionResult GetValues()
        {
            return Ok(new string[] { "value1", "value2" });
        }
    }
}
