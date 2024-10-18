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
    public class InventoryController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;


        public InventoryController(ByteTechSchoolERPContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateInventory()
        { 
            ViewBag.LedgerList = new SelectList(_context.Ledger_Accounts.Select(x => new { Id = x.Id, Name = x.Ledger_Account_Title }), "Id", "Name");

            return View();
        }
        // Handle form submission and save data in DB
        [HttpPost]
        public JsonResult Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Inventories.Add(inventory);
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid data" });
        }
        [HttpGet]
        public JsonResult GetInventoryData()
        {
            var data = _context.Inventories.Include(x => x.Ledger_Account).ToList();
            var inventoryData = data.Select(l => new
            {
                l.Item_Name,
                l.OpeningQuantity,
                Ledger_Account_Title = l.Ledger_Account?.Ledger_Account_Title,
                l.UnitOfMeasure,
                l.Id,
                l.Ledger_AccountId
            }).ToList();
            return Json(new { data = inventoryData });
        }

        [HttpGet]
        public JsonResult GetInventoryById(int id)
        {
            var inventory = _context.Inventories.FirstOrDefault(x => x.Id == id);
            if (inventory == null)
            {
                return Json(new { success = false, message = "Inventory not found" });
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    inventory.Item_Name,
                    inventory.OpeningQuantity,
                    inventory.Ledger_AccountId,
                    inventory.UnitOfMeasure
                }
            });
        }
        [HttpPost]
        public JsonResult Update(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                var existingInventory = _context.Inventories.FirstOrDefault(x => x.Id == inventory.Id);
                if (existingInventory != null)
                {
                    existingInventory.Item_Name = inventory.Item_Name;
                    existingInventory.OpeningQuantity = inventory.OpeningQuantity;
                    existingInventory.Ledger_AccountId = inventory.Ledger_AccountId;
                    existingInventory.UnitOfMeasure = inventory.UnitOfMeasure;

                    _context.SaveChanges();
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Inventory not found" });
            }

            return Json(new { success = false, message = "Invalid data" });
        }
        [HttpPost]
        public JsonResult DeleteInventory(int id)
        {
            var inventory = _context.Inventories.FirstOrDefault(l => l.Id == id);
            if (inventory == null)
            {
                return Json(new { success = false, message = "Inventory item not found." });
            }

            _context.Inventories.Remove(inventory);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
