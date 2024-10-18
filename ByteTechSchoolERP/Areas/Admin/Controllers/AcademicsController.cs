using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class AcademicsController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AcademicsController(ByteTechSchoolERPContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ClassTimetable()
        {
            try
            {
                // Retrieve data from the database
                ViewBag.Clasess = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
                ViewBag.Sections = await _context.Sections.ToListAsync();
                ViewBag.subject = new SelectList(_context.SubjectModels.Select(x => new { Id = x.Id, Name = x.SubjectName }), "Id", "Name");
                ViewBag.StaffTempId = new SelectList(_context.StaffTemps.Select(x => new { Id = x.Id, Name = x.FirstName }), "Id", "Name");

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception and handle the error as needed
                // Log exception (for example, use ILogger)
                Console.WriteLine(ex.Message); // Replace with your logging mechanism
                return View("Error"); // Return an error view or handle the error
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveTimetable([FromBody] ClassTimetableViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Timetable data is required.");
            }

            // Check if IUnitOfWork.Classtimetable is null
            if (_unitOfWork?.Classtimetable == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Class timetable service is not available.");
            }

            // Call the service to save the timetable
            var response = await _unitOfWork.Classtimetable.SaveTimetableAsync(model);

            // Check if response is null or not
            if (response == null)
            {
                return Json(new { isSuccess = false, message = "An error occurred while processing your request." });
            }

            if (response.isSuccess)
            {
                // Set a success message using TempData
                TempData["SuccessMessage"] = response.Message ?? "Operation successful.";

                // Redirect to the StudentInformation action
                return RedirectToAction("ClassTimetable", "Academics");
            }
            else
            {
                // Return JSON with failure message for AJAX
                return Json(new { isSuccess = false, message = response.Message ?? "Validation failed." });
            }
        }

        public async Task<IActionResult> TeacherTimetable()
        {
            ViewBag.StaffTempId = new SelectList(_context.StaffTemps.Select(x => new { Id = x.Id, Name = x.FirstName }), "Id", "Name");

            return View();
        }
        public async Task<IActionResult> AssignClassTeacher()
        {
            return View();
        }
        [HttpGet]

        public IActionResult GetTimetable(Guid staffTempId)
        {
            // Query TimetableEntries
            var timetableEntries = _context.TimetableEntries
                .Where(te => te.StaffTempId == staffTempId)
                .Select(te => new
                {
                    te.ClassTimetable.ClassId,
                    te.SubjectId,
                    te.TimeFrom,
                    te.TimeTo,
                    te.RoomNo,
                    te.ClassTimetable.SectionId,
                    te.ClassTimetable.Day
                })
                .ToList();

            // Query Class and Subject names
            var classIds = timetableEntries.Select(te => te.ClassId).Distinct().ToList();
            var subjectIds = timetableEntries.Select(te => te.SubjectId).Distinct().ToList();

            var classes = _context.Classes
                .Where(c => classIds.Contains(c.Id))
                .ToDictionary(c => c.Id, c => c.ClassName);

            var subjects = _context.SubjectModels
                .Where(s => subjectIds.Contains(s.Id))
                .ToDictionary(s => s.Id, s => s.SubjectName);

            var sections = _context.Sections
                .Where(sec => timetableEntries.Select(te => te.SectionId).Contains(sec.Id))
                .ToDictionary(sec => sec.Id, sec => sec.Name);

            // Map timetableEntries to include ClassName, SubjectName, and SectionName
            var result = timetableEntries.Select(te => new
            {
                ClassId = te.ClassId,
                ClassName = classes.ContainsKey(te.ClassId) ? classes[te.ClassId] : "Unknown",
                SubjectId = te.SubjectId,
                SubjectName = subjects.ContainsKey(te.SubjectId) ? subjects[te.SubjectId] : "Unknown",
                SectionId = te.SectionId,
                SectionName = sections.ContainsKey(te.SectionId) ? sections[te.SectionId] : "Unknown",
                TimeFrom = te.TimeFrom,
                TimeTo = te.TimeTo,
                RoomNo = te.RoomNo,
                DayOfWeek = te.Day
            }).ToList();

            return Json(new { data = result });
        }




    }
}
