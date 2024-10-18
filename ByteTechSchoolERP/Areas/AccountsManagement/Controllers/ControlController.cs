using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ByteTechSchoolERP.Areas.AccountsManagement.Controllers
{
    [Area("AccountsManagement")]
    [AuthenticationFilter]

    public class ControlController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;


        public ControlController(ByteTechSchoolERPContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateControl()
        {

            ViewBag.AccountList = new SelectList(_context.Element_Accounts.Select(x => new { Id = x.Id, Name = x.Account_Title }), "Id", "Name");
            return View();
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Create(Control_Account data)
        {
            if (data != null)
            {
                _context.Control_Accounts.Add(data);
                _context.SaveChanges();

                return Json(new { success = true, });
            }

            return Json(new { success = false, message = "Error creating control account." });
        }


        [HttpGet]
        public JsonResult GetControlData()
        {
            var data = _context.Control_Accounts.Include(x => x.Element_Account).ToList();
            var AccountData = data.Select(l => new
            {
                l.Element_Account.Account_Title,
                l.Control_Complete_Code,
                l.Control_Account_Title,
                l.Id
            });
            return Json(new { data = AccountData });
        }




        public ActionResult Add_Control_Account_Json(int EleAcc)
        {

            var controlAc = _context.Control_Accounts
       .Where(x => x.Element_AccountId == EleAcc)
       .Select(x => (double?)x.Control_Account_Code)
       .Max() ?? 0;

            if (controlAc == 0)
            {
                return Json(0);
            }
            else
            {
                return Json(controlAc);

            }


        }
        [HttpGet]
        public IActionResult GetControlById(int id)
        {
            var controlAccount = _context.Control_Accounts
                .FirstOrDefault(ca => ca.Id == id);

            if (controlAccount == null)
            {
                return NotFound();
            }

            return Json(new
            {
                element_AccountId = controlAccount.Element_AccountId,
                control_Account_Title = controlAccount.Control_Account_Title,
                control_Complete_Code = controlAccount.Control_Complete_Code,
                id = controlAccount.Id
            });
        }
        [HttpPost]
        public JsonResult UpdateControl(Control_Account model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var controlAccount = _context.Control_Accounts.FirstOrDefault(x => x.Id == model.Id);
                    if (controlAccount != null)
                    {
                        controlAccount.Element_AccountId = model.Element_AccountId;
                        controlAccount.Control_Complete_Code = model.Control_Complete_Code;
                        controlAccount.Control_Account_Title = model.Control_Account_Title;

                        _context.SaveChanges();

                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Control account not found." });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    return Json(new { success = false, message = "Model state is not valid.", errors = errors });
                }
            }
            catch (Exception ex)
            {
                // Log the exception message
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult DeleteControl(int id)
        {
            try
            {
                var controlAccount = _context.Control_Accounts.FirstOrDefault(ca => ca.Id == id);
                if (controlAccount != null)
                {
                    _context.Control_Accounts.Remove(controlAccount);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Control account not found." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception message
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
