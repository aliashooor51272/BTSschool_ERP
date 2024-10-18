using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.InstitudesProfile;
using ByteTechSchoolERP.Models.ViewModels;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class GeneralSettingController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GeneralSettingController> _logger;

        public GeneralSettingController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, ILogger<GeneralSettingController> logger)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InstituteProfile()
        {
            TempData.Keep("InstituteData");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddInstitute(Instituite_VM institute)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { isSuccess = false, message = "Validation errors occurred.", errors });
            }

            try
            {
                
                var response = await _unitOfWork.Institute.AddIntitute(institute);

                if (response.isSuccess)
                {
                    // Store data in TempData
                    TempData["InstituteData"] = JsonConvert.SerializeObject(institute);
                    TempData["InstituteData"] = JsonConvert.SerializeObject(new
                    {
                        institute.Name,
                        institute.PhoneNumber,
                        institute.WebsiteUrl,
                        institute.Address,
                        institute.Country,
                        institute.City,
                        ImagePath =  response.Message // Assuming the image path is saved in response.Message
                    });

                    return RedirectToAction("InstituteProfile", "GeneralSetting");
                }
                else
                {
                    return Json(new { isSuccess = false, message = response.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = $"An error occurred: {ex.Message}" });
            }
        }
        public async Task<IActionResult> AddRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

     

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("AddRole");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }
        public async Task<IActionResult> FeeParticulars()
        {
            return View();
        }
        public async Task<IActionResult> MarksGrading()
        {
            return View();
        }
        public async Task<IActionResult> FeeChallanDetails()
        {
            return View();
        }
        public async Task<IActionResult> RulesAndRegulation()
        {
            return View();
        }
        public async Task<IActionResult> AccountSettings()
        {
            return View();
        }
    }
}
