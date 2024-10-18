using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.Lesson;
using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class LessonPlanController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LessonPlanController(ByteTechSchoolERPContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddLesson()
        {
            ViewBag.Clasess = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");

            var sections = await _context.Sections.ToListAsync();
            ViewBag.Sections = sections.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();
            ViewBag.Subject = new SelectList(_context.SubjectModels.Select(x => new { Id = x.Id, Name = x.SubjectName }), "Id", "Name");

            var classes = await _context.Classes.ToListAsync();
            var sectionss = await _context.Sections.Include(s => s.Class).ToListAsync();

            ViewBag.Classes = classes;
            ViewBag.Sections = sections;

            ViewBag.Classes = classes;
           

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> CreateLesson(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.Lessons.AddOrUpdateLessons(lesson);
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

            return View("AddLesson", lesson);
        }


 

        public async Task<IActionResult> LessonList()
        {
            var lessonList = _unitOfWork.Lessons.GetLessonList(null);

             return Json(new { data = lessonList });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var response = await _unitOfWork.Lessons.DeleteLessonById(id);
            return Json(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await _context.Classes.Select(c => new { c.Id, c.ClassName }).ToListAsync();
            return Json(classes);
        }

        [HttpGet]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _context.Sections.Select(s => new { s.Id, s.Name }).ToListAsync();
            return Json(sections);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _context.SubjectModels.Select(s => new { s.Id, s.SubjectName }).ToListAsync();
            return Json(subjects);
        }
        [HttpGet]
        public async Task<IActionResult> GetSectionsByClassId(Guid classId)
        {
            var sections = await _context.Sections
                                          .Where(s => s.ClassId == classId)
                                          .Select(s => new { s.Id, s.Name })
                                          .ToListAsync();
            return Json(sections);
        }



    }

}
