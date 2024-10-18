using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Campus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using ByteTechSchoolERP.Models.SubAdmin;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Core;

using ByteTechSchoolERP.DataAccess.Filters;

using ByteTechSchoolERP.Models.Students;
using ByteTechSchoolERP.Models.Subjects;
using Microsoft.Extensions.Hosting.Internal;
namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubAdminRegisterController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ByteTechSchoolERPUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SubAdminRegisterController(ByteTechSchoolERPContext context, IUnitOfWork unitOfWork, UserManager<ByteTechSchoolERPUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SubAdminEditViewModel model)
        {
            
            
                var subAdmin = await _context.SubAdminn.FindAsync(model.Id);
                if (subAdmin == null)
                {
                    return NotFound();
                }

                subAdmin.UserName = model.UserName;
                subAdmin.Email = model.Email;
                subAdmin.PhoneNumber = model.PhoneNumber;
                subAdmin.CNICNumber = model.CNICNumber;
                subAdmin.CampusName = model.CampusName;
                subAdmin.Status = model.Status;
                _context.SubAdminn.Update(subAdmin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Create"); // Redirect to the list view
            

   
        }


        public IActionResult Create()
        {
            var campuses = _context.SchoolCampus
         .Select(c => new SelectListItem
         {
             Value = c.CampusId.ToString(),
             Text = c.CampusName
         }).ToList();
            ViewBag.Campuses = campuses; // Ensure this is not null

            var subAdmins = _context.SubAdminn.ToList(); // Ensure _context is properly configured
            return View(subAdmins);
        }  // POST: Admin/SubAdminRegister/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSubAdmin(SubAdminn model)
        {


            // Convert file to byte array if provided


            // Create a new SubAdmin entity
            var subAdmin = new SubAdminn
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CNICNumber = model.CNICNumber,
                CampusName = model.CampusName,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Status = true,
                //Attachment = fileData // Store file data as byte array
            };

            // Add SubAdmin to the database
            _context.SubAdminn.Add(subAdmin); // Ensure SubAdmins DbSet is defined in your DbContext
            await _context.SaveChangesAsync();

            return RedirectToAction("Create"); // Redirect to a success page or list
        }



        // GET: Admin/SubAdminRegister/Create
        [HttpGet]
        public IActionResult CreateCampus()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCampus(SchoolCampus model)
        {
            if (ModelState.IsValid)
            {
                var campus = new SchoolCampus
                {
                    CampusId = Guid.NewGuid(),
                    CampusName = model.CampusName,
                    City = model.City,
                    Area = model.Area,
                    Description = model.Description
                };

                _context.SchoolCampus.Add(campus);
                await _context.SaveChangesAsync();

                return RedirectToAction("CreateCampus");
            }

            return View(model);
        }
        public async Task<IActionResult> CampusList()
        {
         
            var campusList = await _context.SchoolCampus.ToListAsync();
            return Json(new { data = campusList });
        }
        // POST: Admin/SubAdminRegister/EditCampus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCampus(SchoolCampus model)
        {
            if (ModelState.IsValid)
            {
                var campus = await _context.SchoolCampus.FindAsync(model.CampusId);
                if (campus != null)
                {
                    campus.CampusName = model.CampusName;
                    campus.City = model.City;
                    campus.Area = model.Area;
                    campus.Description = model.Description;

                    _context.SchoolCampus.Update(campus);
                    await _context.SaveChangesAsync();

                    return Json(new { isSuccess = true });
                }
                return Json(new { isSuccess = false, message = "Campus not found." });
            }
            return Json(new { isSuccess = false, message = "Invalid data." });
        }

        // POST: Admin/SubAdminRegister/DeleteCampus
        [HttpPost]
        public async Task<IActionResult> DeleteCampuses(Guid campusId)
        {
            var campus = await _context.SchoolCampus.FindAsync(campusId);
            if (campus != null)
            {
                _context.SchoolCampus.Remove(campus);
                await _context.SaveChangesAsync();
                return Json(new { isSuccess = true });
            }
            return Json(new { isSuccess = false, message = "Campus not found." });
        }
   
       

          // Reload campuses for the view in case of validation errors
         
       

        // GET: Admin/SubAdminRegister/Index
        public IActionResult Index()
        {
            return View();
        }







    }
}
