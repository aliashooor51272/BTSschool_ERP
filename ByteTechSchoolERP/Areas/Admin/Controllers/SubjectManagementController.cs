using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.ViewModels;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class SubjectManagementController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectManagementController(ByteTechSchoolERPContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _unitOfWork = unitOfWork;

        }
        public IActionResult GetSubjectAllocations()
        {
            var data = _context.SubjectAllocations
                .Select(sa => new
                {
                    sa.Id,
                    Teacher = _context.Employee.Where(t => t.EmployeeId == sa.TeacherId).Select(t => t.FirstName + " " + t.LastName).FirstOrDefault(),
                    Class = _context.Classes.Where(c => c.Id == sa.ClassId).Select(c => c.ClassName).FirstOrDefault(),
                    Subject = _context.SubjectModels.Where(s => s.Id == sa.SubjectId).Select(s => s.SubjectName).FirstOrDefault(),
                    Section = _context.Sections.Where(s => s.Id == sa.SectionId).Select(s => s.Name).FirstOrDefault(),
                    CreatedOn = sa.Createdon
                }).ToList();

            return Json(new { data });
        }

        public async Task<IActionResult> AssignSubject()
        {
            // Get the current user ID
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacherRole = await _context.Roles
                                .Where(r => r.Name == "Teacher")
                                .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                                .FirstOrDefaultAsync();
            var teacher = teacherRole.Id;

            // Fetch classes and teachers as before
            var classes = _context.Classes

                .Select(c => new SelectListItem
                {
                    Text = c.ClassName,
                    Value = c.Id.ToString()
                }).ToList();

            var teachers = _context.Employee
     .Where(t => t.RoleId == teacher)
     .Select(t => new
     {
         Id = t.EmployeeId,
         Name = t.FirstName + " " + t.LastName
     })
     .ToList();


            ViewBag.Classes = new SelectList(classes, "Value", "Text");
            ViewBag.Teachers = new SelectList(teachers, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignSubject(SubjectAllocation model)
        {

            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create a new SubjectAllocation object with the selected values
            var subjectAllocation = new SubjectAllocation
            {
                Id = Guid.NewGuid(),
                TeacherId = model.TeacherId,
                ClassId = model.ClassId,
                SectionId = model.SectionId,
                SubjectId = model.SubjectId,
                UserId = currentUserId,
                Createdon = DateTime.Now
            };

            // Add the new SubjectAllocation object to the database
            _context.SubjectAllocations.Add(subjectAllocation);
            await _context.SaveChangesAsync();

            // Redirect or return a success message
            return RedirectToAction("AssignSubject");
        }




        public IActionResult AddSubject()
        {
            // Get the current user ID
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch classes where UserId matches the current user
            var classes = _context.Classes

                .Select(c => new SelectListItem
                {
                    Text = c.ClassName,
                    Value = c.Id.ToString()
                }).ToList();



            // Pass the class and section data to the view using ViewBag
            ViewBag.Classes = new SelectList(classes, "Value", "Text");


            return View();
        }
        public IActionResult GetSubjectsBySection(Guid sectionId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var subjects = _context.SubjectModels
                .Where(s => s.SectionId == sectionId)
                .Select(s => new { s.Id, s.SubjectName })
                .ToList();

            return Json(subjects);
        }


        public IActionResult GetSectionsByClass(Guid classId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sections = _context.Sections
                .Where(s => s.ClassId == classId)
                .Select(s => new { s.Id, s.Name })
                .ToList();

            return Json(sections);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject(SubjectModel subjectModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Subject.AddOrUpdateSubject(subjectModel);
                if (response.isSuccess)
                {
                    TempData["SuccessMessage"] = response.Message;
                    return Json(new { isSuccess = true, message = response.Message });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "Validation failed." });
                }
            }

            return View("AddSubject", subjectModel);
        }


        public IActionResult SubjectList()
        {
            // Call GetSubjectLists method
            var subjectList = _unitOfWork.Subject.GetSubjectLists(null);

            // Returning JSON result for DataTables
            return Json(new { data = subjectList });
        }

        // New Action for Deleting a Subject
        [HttpPost]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var response = await _unitOfWork.Subject.DeleteSubjectById(id);
            return Json(response);
        }

    }
}