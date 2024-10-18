using ByteTechSchoolERP.DataAccess.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ByteTechSchoolERP.Areas.Teacher.Controllers
{
	[Area("Teacher")]
	[AuthenticationFilter]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
      
    }
}
