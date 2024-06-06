using Microsoft.AspNetCore.Mvc;

namespace MSIT158_2_FinalProject.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
