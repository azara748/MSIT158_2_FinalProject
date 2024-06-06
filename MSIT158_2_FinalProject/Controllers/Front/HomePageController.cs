using Microsoft.AspNetCore.Mvc;

namespace MSIT158_2_FinalProject.Controllers.Front
{
	public class HomePageController : Controller
	{
		public IActionResult Search()
		{
			return View();
		}
	}
}
