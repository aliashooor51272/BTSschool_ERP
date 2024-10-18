using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Areas.AccountsManagement.Controllers
{
    [Area("AccountsManagement")]
    [AuthenticationFilter]
    public class LedgerController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;


        public LedgerController(ByteTechSchoolERPContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateLedger()
        {
            ViewBag.ElementAccountList = new SelectList(_context.Element_Accounts.Select(x => new { Id = x.Id, Name = x.Account_Title }), "Id", "Name");
            ViewBag.ControlAccountList = new SelectList(_context.Control_Accounts.Select(x => new { Id = x.Id, Name = x.Control_Account_Title }), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult CreateLedger(Ledger_Account la)
        {
            if (la != null)
            {
                _context.Ledger_Accounts.Add(la);
                _context.SaveChanges();
                //return RedirectToAction("CreateLedger", "Ledger", new { area = "AccountsManagement" });
            }
            return RedirectToAction("CreateLedger", "Ledger", new { area = "AccountsManagement" });
            //ViewBag.ElementAccountList = new SelectList(_context.Element_Accounts.Select(x => new { Id = x.Id, Name = x.Account_Title }), "Id", "Name");
            //ViewBag.ControlAccountList = new SelectList(_context.Control_Accounts.Select(x => new { Id = x.Id, Name = x.Control_Account_Title }), "Id", "Name");


        }


        [HttpGet]
        public JsonResult GetLedgerData()
        {
            var data = _context.Ledger_Accounts
                 .Include(x => x.Element_Account)
                 .Include(x => x.Control_Account)
                 .Select(l => new
                 {
                     account_Title = l.Element_Account.Account_Title,
                     control_Account_Title = l.Control_Account.Control_Account_Title,
                     l.Ledger_Complete_Code,
                     l.Ledger_Account_Title,
                     l.Balance,
                     l.Id
                 }).ToList();

            return Json(new { data });
        }




        public JsonResult GetElementItems(int elementId)
        {

            var items = _context.Control_Accounts
                 .Where(x => x.Element_AccountId == elementId)
                 .Select(x => new
                 {
                     id = x.Id,
                     name = x.Control_Account_Title
                 }).ToList();

            return Json(items);
        }

        public ActionResult Add_Ledger_Account_Json(int EleAcc, int ConAcc)
        {
            var maxCode = _context.Ledger_Accounts
                .Where(x => x.Control_AccountId == ConAcc && x.Element_AccountId == EleAcc)
                .Select(x => (double?)x.Ledger_Account_Code).Max() ?? 0;

            return Json(maxCode + 1);
        }

        // Fetch the details of the ledger account for the specified id
        [HttpGet]
        public JsonResult GetLedgerById(int id)
        {
            ViewBag.ElementAccountList = new SelectList(_context.Element_Accounts.Select(x => new { Id = x.Id, Name = x.Account_Title }), "Id", "Name");
            ViewBag.ControlAccountList = new SelectList(_context.Control_Accounts.Select(x => new { Id = x.Id, Name = x.Control_Account_Title }), "Id", "Name");
            var ledger = _context.Ledger_Accounts.FirstOrDefault(l => l.Id == id);

            if (ledger == null)
            {
                return Json(new { success = false, message = "Ledger not found." });
            }

            // Return the necessary fields
            return Json(new
            {
                success = true,
                data = new
                {
                    Element_AccountId = ledger.Element_AccountId,
                    Control_AccountId = ledger.Control_AccountId,
                    Ledger_Complete_Code = ledger.Ledger_Complete_Code,
                    Ledger_Account_Title = ledger.Ledger_Account_Title,
                    Balance = ledger.Balance,

                }

            });
        }
        [HttpPost]
        public JsonResult UpdateLedger(Ledger_Account model, int id)
        {
            // Fetch the ledger using the correct id
            var ledger = _context.Ledger_Accounts.FirstOrDefault(l => l.Id == id);

            // Check if the ledger exists
            if (ledger == null)
            {
                return Json(new { success = false, message = "Ledger not found." });
            }

            // Update the fields of the ledger from the model
            ledger.Element_AccountId = model.Element_AccountId;
            ledger.Control_AccountId = model.Control_AccountId;
            ledger.Ledger_Account_Title = model.Ledger_Account_Title;
            ledger.Ledger_Complete_Code = model.Ledger_Complete_Code;
            ledger.Balance = model.Balance;

            // Save changes to the database
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteLedger(int id)
        {
            var ledger = _context.Ledger_Accounts.FirstOrDefault(l => l.Id == id);
            if (ledger == null)
            {
                return Json(new { success = false, message = "Ledger not found." });
            }

            _context.Ledger_Accounts.Remove(ledger);
            _context.SaveChanges();

            return Json(new { success = true });
        }


    }
}
                 
            



    

