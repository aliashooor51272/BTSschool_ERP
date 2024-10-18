using ByteTechSchoolERP.DataAccess.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ByteTechSchoolERP.Areas.Payrolls.Controllers
{
    [Area("Payrolls")]
    [AuthenticationFilter]
    public class AddSalaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddNewSalary()
        {
            return View();
        }
    }
}
