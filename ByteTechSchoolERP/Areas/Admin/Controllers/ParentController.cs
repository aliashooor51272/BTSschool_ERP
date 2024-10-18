using ByteTechSchoolERP.DataAccess.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class ParentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ManageAccount()
        {
            return View();
        }

    }
}
