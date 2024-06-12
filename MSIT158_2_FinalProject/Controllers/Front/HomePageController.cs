using Microsoft.AspNetCore.Mvc;

namespace MSIT158_2_FinalProject.Controllers.Front
{
	public class HomePageController : Controller
	{
		public IActionResult Search()
		{
			return View();
		}
		public IActionResult index(int? id)
		{
			if (id == null||id<=0) { return RedirectToAction("Search"); }
			ViewBag.Id = id;
			return View();
		}
	}
}
