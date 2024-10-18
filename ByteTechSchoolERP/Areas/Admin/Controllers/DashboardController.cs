using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.Models.SubAdmin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class DashboardController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(ByteTechSchoolERPContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var subAdminId = _httpContextAccessor.HttpContext.Session.GetString("SubAdminnId");

            if (!string.IsNullOrEmpty(subAdminId))
            {
                // Fetch the SubAdminn details
                var subAdmin = _context.SubAdminn.FirstOrDefault(s => s.Id.ToString() == subAdminId);

                if (subAdmin != null)
                {
                    ViewBag.SubAdminn = subAdmin;
                }
            }

            return View();
        }

        public IActionResult Teacher()
        {
            return View();
        }

        public IActionResult Parent()
        {
            return View();
        }
    }
}
