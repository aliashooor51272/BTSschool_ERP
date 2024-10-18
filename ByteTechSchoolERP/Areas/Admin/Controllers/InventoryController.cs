using ByteTechSchoolERP.DataAccess.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class InventoryController : Controller
    {
       
      public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> IssueItem()
        {
            return View();
        }
        public async Task<IActionResult> AddItemStock()
        {
            return View();
        }
        public async Task<IActionResult> AddItem()
        {
            return View();
        }
        public async Task<IActionResult> ItemCategory()
        {
            return View();
        }
        public async Task<IActionResult> ItemStore()
        {
            return View();
        }
        public async Task<IActionResult> ItemSupplier()
        {
            return View();
        }
    }
}
