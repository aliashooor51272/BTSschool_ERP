using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using Microsoft.AspNetCore.Mvc;
using ByteTechSchoolERP.Models.ViewModels;
using ByteTechSchoolERP.Models.HomeDiary;
using ByteTechSchoolERP.Models.Students;
using Microsoft.EntityFrameworkCore;
namespace ByteTechSchoolERP.Areas.Parent.Controllers
{
    [Area("Parent")]
    [AuthenticationFilter]
    public class HomeController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ByteTechSchoolERPContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> HomeWorkHistory()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var parentId = _context.Parents
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (parentId == Guid.Empty)
            {
                return NotFound("Parent not found.");
            }

            var students = _context.Students
                .Where(s => s.ParentId == parentId)
                .ToList();

            if (students == null || !students.Any())
            {
                return NotFound("No students found for the parent.");
            }

            // Fetch homework for the students
            var homeWorks = await _context.HomeWork
                .Where(hw => students.Select(s => s.Id).Contains(hw.StudentId))
                .Include(hw => hw.Student)
                .Include(hw => hw.Class)
                .Include(hw => hw.Section)
                .ToListAsync();

            // Create ViewModel
            var viewModel = homeWorks.Select(hw => new HomeWorkViewModel
            {
                Id = hw.Id,
                SubjectName = hw.SubjectName,
                SubmissioDate = hw.SubmissioDate,
                Createdon = hw.Createdon,
                FileUrl = hw.fileurl,
                StudentName = hw.Student.FullName, // Assuming FullName is a property in Student model
                ClassName = hw.Class.ClassName, // Assuming ClassName is a property in Class model
                SectionName = hw.Section.Name // Assuming SectionName is a property in Section model
            }).ToList();

            return View(viewModel);
        }






        [HttpPost]
        public async Task<IActionResult> SubmitHomework(Guid diaryId, Guid classId, Guid sectionId, string subjectId, DateTime submissionDate, IFormFile homeworkFile)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var parentId = _context.Parents
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (parentId == Guid.Empty)
            {
                return NotFound("Parent not found.");
            }

            var students = _context.Students
                .Where(s => s.ParentId == parentId && s.SectionId == sectionId)
                .ToList();

            if (students == null || !students.Any())
            {
                return NotFound("No students found for the parent in the specified section.");
            }

            if (homeworkFile == null || homeworkFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var filePath = Path.Combine(uploadsDirectory, homeworkFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await homeworkFile.CopyToAsync(stream);
            }

            foreach (var student in students)
            {
                var homework = new HomeWork
                {
                    DiaryId = diaryId,
                    ClassId = classId,
                    SectionId = sectionId,
                    SubjectName = subjectId,
                    SubmissioDate = submissionDate,
                    fileurl = $"/uploads/{homeworkFile.FileName}",
                    StudentId = student.Id  // Save the StudentId here
                };

                _context.HomeWork.Add(homework);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("HomeWorkHistory", "Home", new { area = "Parent" });


           
        }

        public IActionResult HomeWork()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var parentId = _context.Parents
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (parentId == Guid.Empty)
            {
                return NotFound("Parent not found.");
            }

            var students = _context.Students
                .Where(s => s.ParentId == parentId)
                .ToList();

            if (students == null || !students.Any())
            {
                return NotFound("No students found for the parent.");
            }

            var sectionIds = students.Select(s => s.SectionId).Distinct().ToList();

            var diaryEntries = (from diary in _context.HomeDiary
                                join section in _context.Sections on diary.SectionId equals section.Id
                                join classEntity in _context.Classes on diary.ClassId equals classEntity.Id
                                join subject in _context.SubjectModels on diary.SubjectId equals subject.Id
                                where sectionIds.Contains(diary.SectionId)
                                select new DiaryViewModel
                                {
                                    DiaryId = diary.Id, // Add this line
                                    ClassId = diary.ClassId,
                                    SectonId = diary.SectionId,
                                    HomeworkDate = diary.HomeworkDate,
                                    SubmissioDate = diary.SubmissioDate,
                                    Description = diary.Description,
                                    AttachDocument = diary.AttachDocument,
                                    SectionName = section.Name,
                                    ClassName = classEntity.ClassName,
                                    SubjectName = subject.SubjectName
                                }).ToList();

            return View(diaryEntries);
        }

        public IActionResult Students()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var parentId = _context.Parents
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (parentId == Guid.Empty)
            {
                return NotFound("Parent not found.");
            }

            var students = _context.Students
                .Where(s => s.ParentId == parentId)
                .Select(s => new StudentViewModel
                {
                    FullName = s.FullName,
                    Surname = s.Surname,
                    Gender = s.Gender,
                    AdmissionDate = s.AdmissionDate,
                    // You can map other properties if needed
                })
                .ToList();

            if (!students.Any())
            {
                return NotFound("No students found for the parent.");
            }

            return View(students);
        }


        public IActionResult Attendance()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var parentId = _context.Parents
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (parentId == Guid.Empty)
            {
                return NotFound("Parent not found.");
            }

            var students = _context.Students
                .Where(s => s.ParentId == parentId)
                .ToList();

            if (students == null || !students.Any())
            {
                return NotFound("No students found for the parent.");
            }

            var studentIds = students.Select(s => s.Id).ToList();
            var attendanceRecords = _context.StudentAttendances
                .Where(a => studentIds.Contains(a.StudentId.Value))
                .Select(a => new AttendanceViewModel
                {
                    StudentId = a.StudentId.Value,
                    StudentName = a.Student.FullName,
                    AttendanceDate = a.AttendanceDate,
                    Status = a.Status,
                    Remarks = a.Remarks
                })
                .ToList();

            return View(attendanceRecords);
        }

        public async Task<IActionResult> StudentMarks()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var parentId = _context.Parents
                .Where(p => p.UserId == userId)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (parentId == Guid.Empty)
            {
                return NotFound("Parent not found.");
            }


            // Fetch marks data including related student, class, and section information
            var marksData = await _context.Marks
                    .Include(m => m.Student)
                    .Include(m => m.Student.Class)
                    .Include(m => m.Student.Section)
                    .Select(m => new
                    {
                        m.Id,
                        m.TotalMarks,
                        m.ObtainMarks,
                        m.Grade,
                        m.Percentage,
                        StudentName = m.Student.FullName,
                        ClassName = m.Student.Class.ClassName,
                        SectionName = m.Student.Section.Name,
                        SubjectId = m.Subject
                    })
                    .ToListAsync();

                // Fetch subject names and create a dictionary for lookup
                var subjects = await _context.SubjectModels
                    .ToDictionaryAsync(s => s.Id, s => s.SubjectName);

                // Map the data to MarksViewModel
                var marksViewModel = marksData.Select(m => new MarksViewModel
                {

                    StudentName = m.StudentName,
                    SubjectName = subjects.TryGetValue(m.SubjectId.GetValueOrDefault(), out var subjectName) ? subjectName : "Unknown",
                    ClassName = m.ClassName,
                    SectionName = m.SectionName,
                    TotalMarks = m.TotalMarks.GetValueOrDefault(),
                    ObtainedMarks = m.ObtainMarks.GetValueOrDefault(),
                    Grade = m.Grade,
                    Percentage = m.Percentage.GetValueOrDefault()
                }).ToList();

                return View(marksViewModel);
            }


    }
}