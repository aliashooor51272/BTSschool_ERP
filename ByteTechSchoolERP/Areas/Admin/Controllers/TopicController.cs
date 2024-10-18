using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.Topic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TopicController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public TopicController(ByteTechSchoolERPContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddTopic()
        {
            ViewBag.Classes = new SelectList(await _context.Classes.Select(x => new { x.Id, x.ClassName }).ToListAsync(), "Id", "ClassName");
            ViewBag.Sections = new SelectList(await _context.Sections.Select(x => new { x.Id, x.Name }).ToListAsync(), "Id", "Name");
            ViewBag.Subjects = new SelectList(await _context.SubjectModels.Select(x => new { x.Id, x.SubjectName }).ToListAsync(), "Id", "SubjectName");
            ViewBag.Lessons = new SelectList(await _context.Lessons.Select(x => new { x.Id, x.LessonName }).ToListAsync(), "Id", "LessonName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTopic(Topic topic)
        {
            var response = await _unitOfWork.Topic.AddOrUpdateTopic(topic);
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

        [HttpGet]
        public IActionResult TopicList()
        {
            var topics = _unitOfWork.Topic.GetTopicList();
            var topicList = topics.Select(s => new
            {
                Id = s.Id,
                ClassName = _context.Classes.FirstOrDefault(c => c.Id == s.ClassId)?.ClassName ?? "Unknown Class",
                SectionName = _context.Sections.FirstOrDefault(sec => sec.Id == s.SectionId)?.Name ?? "Unknown Section",
                SubjectName = _context.SubjectModels.FirstOrDefault(sub => sub.Id == s.SubjectId)?.SubjectName ?? "Unknown Subject",
                LessonName = _context.Lessons.FirstOrDefault(les => les.Id == s.LessonId)?.LessonName ?? "Unknown Lesson",
                TopicName = s.Topic,
            }).ToList();

            return Json(new { data = topicList });
        }
    }
}
