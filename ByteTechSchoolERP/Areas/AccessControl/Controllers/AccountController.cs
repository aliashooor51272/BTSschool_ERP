using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ByteTechSchoolERP.Controllers
{
    [Area("AccessControl")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ByteTechSchoolERPUser> _signInManager;
        private readonly UserManager<ByteTechSchoolERPUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ByteTechSchoolERPContext _context;

        public AccountController(SignInManager<ByteTechSchoolERPUser> signInManager, ByteTechSchoolERPContext context, UserManager<ByteTechSchoolERPUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the credentials match a SubAdmin
                var subAdmin = await _context.SubAdminn
                    .FirstOrDefaultAsync(s => s.Email == model.Username && s.Password == model.Password);

                if (subAdmin != null)
                {// Store the SubAdminn ID in the session
                    _httpContextAccessor.HttpContext.Session.SetString("SubAdminnId", subAdmin.Id.ToString());

                    // Redirect to the SubAdmin dashboard and stop further execution
                    return Redirect("/Admin/Dashboard/Index");
                }

                // Check if the credentials match an IdentityUser
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = await _userManager.GetRolesAsync(user);

                    _httpContextAccessor.HttpContext.Session.SetString("Username", model.Username);
                    _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);
                    _httpContextAccessor.HttpContext.Session.SetString("UserRoleId", roles[0]);

                    if (roles.Contains("Admin"))
                    {
                        return Redirect("/Admin/Dashboard/Index");
                    }
					if (roles.Contains("Teacher"))
					{
						return Redirect("/Teacher/Home/Index");
					}
                    if (roles.Contains("Parent"))
                    {
                        return Redirect("/Parent/Home/Index");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            // If we got this far, something failed, redisplay the form
            return View(model);
        }

    }
}
