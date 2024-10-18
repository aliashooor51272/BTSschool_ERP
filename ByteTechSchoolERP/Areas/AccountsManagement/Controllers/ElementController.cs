using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.DataAccess.Repository.UnitOfWork;
using ByteTechSchoolERP.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ByteTechSchoolERP.Areas.AccountsManagement.Controllers
{
    [Area("AccountsManagement")]
    [AuthenticationFilter]
    public class ElementController : Controller
    {

        private readonly ByteTechSchoolERPContext _context;


        public ElementController(ByteTechSchoolERPContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateElement()
        {

            int maxElementAccountCode = _context.Element_Accounts.Max(e => (int?)e.Element_Account_Code) ?? 0;
            int nextElementAccountCode = maxElementAccountCode + 1;

            // Create a new instance of Element_Account and set Element_Account_Code to the next incremented value
            var model = new Element_Account { Element_Account_Code = nextElementAccountCode };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateElement(Element_Account model)
        {

            if (ModelState.IsValid)
            {
                _context.Element_Accounts.Add(model);
                _context.SaveChanges();
                
                return RedirectToAction("CreateElement", "Element", new { area = "AccountsManagement" });
            }
            
            return RedirectToAction("CreateElement", "Element");

        }


        [HttpGet]
        public JsonResult GeElementData()
        {
            // var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            var data = _context.Element_Accounts // Filter by UserId
                               .ToList();

            var ElementData = data.Select(l => new
            {
                l.Id,
                l.Account_Title,
                l.Element_Account_Code
            });

            return Json(new { data = ElementData });
        }
        [HttpPost]
        public JsonResult UpdateElement(int elementId, string accountTitle, int accountCode)
        {
            {
                var element = _context.Element_Accounts.Find(elementId);

                if (element != null)
                {
                    element.Account_Title = accountTitle;
                    // element.Element_Account_Code remains unchanged
                   // element.Element_Account_Code =(int)accountCode;
                    _context.SaveChanges();
                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
        }
        [HttpPost]
        public JsonResult DeleteElement(int id)
        {
            var element = _context.Element_Accounts.Find(id);

            if (element != null)
            {
                _context.Element_Accounts.Remove(element);
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
