using ByteTechSchoolERP.DataAccess.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class TransportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Iindex()
        {
            return View();
        }
        public async Task<IActionResult> Routes()
        {
            return View();
        }
        public async Task<IActionResult> Vehicle()
        {
            return View();
        }
        public async Task<IActionResult> AssignVehicle()
        {
            return View();
        }
    }
}