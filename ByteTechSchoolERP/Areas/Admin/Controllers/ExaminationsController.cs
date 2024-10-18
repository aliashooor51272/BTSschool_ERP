using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Exam;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class ExaminationsController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExaminationsController(ByteTechSchoolERPContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public IActionResult ExamList()
        {
            var exams = from e in _context.ExamLists
                        join c in _context.Classes on e.ClassId equals c.Id
                        join s in _context.Sections on e.SeactionId equals s.Id
                        join t in _context.Terms on e.TermId equals t.Id
                        join g in _context.Grades on e.GradeId equals g.Id
                        select new
                        {
                            e.Id,
                            e.ExamName,
                            e.Description,
                            ClassName = c.ClassName,
                            SectionName = s.Name,
                            TermName = t.Name,
                            GradeName = g.GradeTitle,
                            e.Session,
                            e.IsPublished,
                            e.AddExamDate

                        };

            return Json(new { data = exams.ToList() });
        }


        public async Task<IActionResult> MarkSheetDesing()
        {
            // Retrieve data from the database
            var examTemplates = await _context.ExamTemplates.ToListAsync();

            // Pass the data to the view
            return View(examTemplates);
        }
        public async Task<IActionResult> DesingCard()
        {
            var examTemplates = await _context.ExamTemplates.ToListAsync();

            // Pass the data to the view
            return View(examTemplates);
        }
        [HttpGet]
        public async Task<IActionResult> GetExamTemplates()
        {
            var examTemplates = await _context.ExamTemplates.ToListAsync();
            return Json(examTemplates);
        }

        public IActionResult CreateExam()
        {
            ViewBag.Subject = _context.SubjectModels.ToList();

            ViewBag.Clasess = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
            ViewBag.Section = new SelectList(_context.Sections.Select(x => new { Id = x.Id, Name = x.Name }), "Id", "Name");
            ViewBag.Terms = new SelectList(_context.Terms.Select(x => new { Id = x.Id, Name = x.Name }), "Id", "Name");
            ViewBag.Grade = new SelectList(_context.Grades.Select(x => new { Id = x.Id, Name = x.GradeTitle }), "Id", "Name");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> CreateExam(ExamList examList)
        {
            var response = await _unitOfWork.Exam.AddOrUpdateExam(examList);
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
        public async Task<IActionResult> GetTheRelatedSection(Guid classId)
        {
            var sections = await _context.Sections
                .Where(s => s.ClassId == classId)
                .Select(s => new { id = s.Id, name = s.Name })
                .ToListAsync();

            return Json(new { data = sections });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            var response = await _unitOfWork.Exam.DeleteExamById(id);
            return Json(new { isSuccess = response });
        }


        [HttpGet]
        public async Task<IActionResult> GetExamById(Guid id)
        {
            var exam = await _context.ExamLists
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    e.Id,
                    e.ExamName,
                    e.Description,
                    e.ClassId,
                    e.SeactionId,
                    e.TermId,
                    e.GradeId,
                    e.Session,
                    e.AddExamDate,
                    e.IsPublished
                })
                .FirstOrDefaultAsync();

            if (exam == null)
            {
                return NotFound();
            }

            return Json(exam);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateExam(ExamList examList)
        {
            if (ModelState.IsValid)
            {
                var existingExam = await _context.ExamLists.FindAsync(examList.Id);
                if (existingExam == null)
                {
                    return NotFound();
                }

                existingExam.ExamName = examList.ExamName;
                existingExam.Description = examList.Description;
                existingExam.ClassId = examList.ClassId;
                existingExam.SeactionId = examList.SeactionId;
                existingExam.TermId = examList.TermId;
                existingExam.GradeId = examList.GradeId;
                existingExam.Session = examList.Session;
                existingExam.AddExamDate = examList.AddExamDate;
                existingExam.IsPublished = examList.IsPublished;

                await _context.SaveChangesAsync();
                return Json(new { isSuccess = true, message = "Exam updated successfully!" });
            }
            return Json(new { isSuccess = false, message = "Validation failed." });
        }



        [HttpPost]
        public async Task<IActionResult> AddSubject(ExamList subject)
        {
            if (ModelState.IsValid)
            {
                _context.ExamLists.Add(subject);
                await _context.SaveChangesAsync();
                return Json(new { isSuccess = true, message = "Subject added successfully!" });
            }
            return Json(new { isSuccess = false, message = "Validation failed." });
        }

        public IActionResult ExamSchedular()
        {
            // Fetch the exam list and populate ViewBag.Exam
            ViewBag.Subject = new SelectList(_context.SubjectModels.Select(x => new { Id = x.Id, Name = x.SubjectName }), "Id", "Name");

            // Ensure there is no issue in fetching the data
            var examSchedules = _context.ExamSchedulars
                                        .Select(x => new ExamSchedular
                                        {
                                            ExamId = x.ExamId,
                                            SubjectId = x.SubjectId,
                                            StartTime = x.StartTime,
                                            Duration = x.Duration,
                                            Date = x.Date,
                                            CreatedDate = x.CreatedDate,
                                            Createdby = x.Createdby,
                                            UpdateDate = x.UpdateDate,
                                            Updateby = x.Updateby,
                                            Roomno = x.Roomno
                                        })

                                        .ToList();

            return View(examSchedules);
        }

        [HttpPost]
        public IActionResult SaveExamSchedular(ExamSchedular model)
        {
            if (ModelState.IsValid)
            {
                _context.ExamSchedulars.Add(model);
                _context.SaveChanges();
                return RedirectToAction("ExamSchedular");
            }
            // If model state is not valid, return to the same view with the model to show validation errors
            return View("ExamSchedular", model);
        }
        [HttpPost]
        public IActionResult SaveExamSchedule(List<ExamSchedular> examSchedules)
        {
            var now = DateTime.Now;
            var username = User.Identity.Name;

            foreach (var schedule in examSchedules)
            {
                // Check if the schedule already exists
                var existingSchedule = _context.ExamSchedulars.FirstOrDefault(e => e.Id == schedule.Id);

                if (existingSchedule != null)
                {
                    // Update existing schedule
                    existingSchedule.ExamId = schedule.ExamId ?? existingSchedule.ExamId;
                    existingSchedule.SubjectId = schedule.SubjectId ?? existingSchedule.SubjectId;
                    existingSchedule.Date = schedule.Date ?? existingSchedule.Date;
                    existingSchedule.StartTime = schedule.StartTime ?? existingSchedule.StartTime;
                    existingSchedule.Duration = schedule.Duration ?? existingSchedule.Duration;
                    existingSchedule.Roomno = schedule.Roomno ?? existingSchedule.Roomno;
                    existingSchedule.UpdateDate = now;
                    existingSchedule.Updateby = username;

                    _context.ExamSchedulars.Update(existingSchedule);
                }
                else
                {
                    // Add new schedule
                    schedule.Id = Guid.NewGuid();
                    schedule.CreatedDate = now;
                    schedule.Createdby = username;
                    _context.ExamSchedulars.Add(schedule);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("CreateExam");
        }

        [HttpGet]
        public IActionResult GetExamSchedules(Guid examId)
        {
            var schedules = _context.ExamSchedulars
                                    .Where(e => e.ExamId == examId)
                                    .Select(e => new
                                    {
                                        e.Id,
                                        SubjectName = _context.SubjectModels.FirstOrDefault(s => s.Id == e.SubjectId).SubjectName,
                                        e.Date,
                                        e.StartTime,
                                        e.Duration,
                                        e.Roomno
                                    })
                                    .ToList();

            return Json(new { data = schedules });
        }
       


    }

}