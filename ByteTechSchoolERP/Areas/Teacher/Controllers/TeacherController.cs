using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ByteTechSchoolERP.Models.Marks;
using ByteTechSchoolERP.Models.HomeDiary; // Add this using directive
namespace ByteTechSchoolERP.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [AuthenticationFilter]
    public class TeacherController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
            
        private readonly IWebHostEnvironment _webHostEnvironment; // Define the variable

        private readonly IHttpContextAccessor _httpContextAccessor;
        public TeacherController(ByteTechSchoolERPContext context, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; // Assign it here
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Result()
        {
            // Fetch data from HomeWork table with related Student data
            var homeWorks = await _context.HomeWork
                                          .Include(hw => hw.Student)
                                            .Include(hw => hw.Class) // Include Class// Include the Student navigation property
                                          .ToListAsync();

            return View(homeWorks);
        }
        public IActionResult DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path cannot be null or empty.");
            }

            // Adjust the path to include the 'images' directory within 'wwwroot'
            var fileFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

            if (!System.IO.File.Exists(fileFullPath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(fileFullPath);
            var fileName = Path.GetFileName(fileFullPath);
            var fileExtension = Path.GetExtension(fileFullPath).ToLowerInvariant();

            // Set content type based on file extension
            var contentType = fileExtension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            // Return file with download header
            return File(fileBytes, contentType, fileName);
        }


        [HttpGet]
        public async Task<IActionResult> GetDiaryData()
        {
            var diaryData = await (from d in _context.HomeDiary
                                   join c in _context.Classes on d.ClassId equals c.Id
                                   join s in _context.Sections on d.SectionId equals s.Id
                                   join subj in _context.SubjectModels on d.SubjectId equals subj.Id
                                   select new DiaryDataViewModel
                                   {
                                       ClassName = c.ClassName,        // Class Name
                                       SectionName = s.Name,           // Section Name
                                       SubjectName = subj.SubjectName, // Subject Name
                                       HomeworkDate = d.HomeworkDate,
                                       SubmissionDate = d.SubmissioDate,
                                       AttachDocument = d.AttachDocument,
                                       Description = d.Description
                                   }).ToListAsync();

            return Json(diaryData);
        }




        public async Task<IActionResult> StudentMarks()
        {
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


        [HttpPost]
        public IActionResult SaveMarks(MarksViewModel model)
        {
           
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var marks = new Marks
                
                {
                    Userid = userId,
                    StudentId = model.StudentId,
                    Subject = model.SubjectId,
                    TotalMarks = model.TotalMarks,
                    ObtainMarks = model.ObtainedMarks,
                    Percentage = model.Percentage,
                    Grade = model.Grade,
                    CreatedOn = DateTime.Now
                };

                _context.Marks.Add(marks);
                _context.SaveChanges();

            // Optionally, redirect to another page or display a success message
            return RedirectToAction("StudentMarks", "Teacher", new { area = "Teacher" });
        }

        // If validation fails, return the same view with validation errors
        public IActionResult ViewFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(Path.Combine("wwwroot", filePath)))
            {
                return NotFound("File not found.");
            }

            var fileStream = new FileStream(Path.Combine("wwwroot", filePath), FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf");
        }

        public IActionResult DailyDairy()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User not found.");
            }

            var staffTempId = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.StaffTempId)
                .FirstOrDefault();

            if (staffTempId == null)
            {
                return NotFound("StaffTempId not found for the current user.");
            }

            // Get allocation details
            var allocations = _context.SubjectAllocations
                .Where(a => a.TeacherId == staffTempId)
                .Select(a => new
                {
                    a.ClassId,
                    a.SectionId,
                    a.SubjectId
                })
                .FirstOrDefault();

            if (allocations == null)
            {
                return NotFound("No allocations found for the current teacher.");
            }

            // Get class, section, and subject data
            var classes = _context.Classes.ToList();
            var sections = _context.Sections.Where(s => s.ClassId == allocations.ClassId).ToList();
            //var subjects = _context.SubjectModels.Where(s => s.ClassId == allocations.ClassId && s.SectonId == allocations.SectionId).ToList();

            // Prepare ViewModel
            var viewModel = new ClassSectionSubjectViewModel
            {
                // Assume default values or set as required for the form
                ClassId = allocations.ClassId.Value,
                ClassName = classes.FirstOrDefault(c => c.Id == allocations.ClassId)?.ClassName,
                SectionId = allocations.SectionId,
                SectionName = sections.FirstOrDefault(s => s.Id == allocations.SectionId)?.Name,
                SubjectId = allocations.SubjectId.Value,
                //SubjectName = subjects.FirstOrDefault(s => s.Id == allocations.SubjectId)?.SubjectName
            };

            // If you need additional data like HomeworkDate, SubmissioDate, etc., set default values or fetch from database

            return View(viewModel);
        }


        public IActionResult ViewDailyDiary()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User not found.");
            }

            var staffTempId = _context.Users
                                       .Where(u => u.Id == userId)
                                       .Select(u => u.StaffTempId)
                                       .FirstOrDefault();

            if (staffTempId == null)
            {
                return NotFound("StaffTempId not found for the current user.");
            }

            var diaryEntries = _context.HomeDiary
                                       .Include(d => d.Class)   // Including navigation property for Class
                                       .Include(d => d.Subject) // Including navigation property for Subject
                                        .Include(d => d.Section) // Including navigation property for Section

                                       .ToList();

            return View(diaryEntries);
        }



        [HttpPost]
        public async Task<IActionResult> SaveDiary(Diary diary, IFormFile AttachDocument)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            diary.UserId = currentUserId;
            if (AttachDocument != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string fileName = Guid.NewGuid().ToString() + "-" + AttachDocument.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await AttachDocument.CopyToAsync(fileStream);
                }

                diary.AttachDocument = "/images/" + fileName;
            }
            
            _context.HomeDiary.Add(diary);
            await _context.SaveChangesAsync();

            return RedirectToAction("DailyDairy", "Teacher", new { area = "Teacher" });
        }

        public IActionResult Marks()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var staffTempId = _context.Users
                                       .Where(u => u.Id == userId)
                                       .Select(u => u.StaffTempId)
                                       .FirstOrDefault();

            if (staffTempId == null)
            {
                return NotFound("StaffTempId not found for the current user.");
            }

            // Fetch the necessary data for the model, including related entities
            var model = _context.SubjectAllocations
                                 .Include(sa => sa.Class)   // Include related Class entity
                                 .Include(sa => sa.Section) // Include related Section entity
                                 .Include(sa => sa.Subject) // Include related Subject entity
                                 .Join(_context.ExamLists,
                                       sa => new { sa.ClassId, sa.SectionId },
                                       el => new { ClassId = el.ClassId, SectionId = el.SeactionId },
                                       (sa, el) => new { sa, el }) // Perform the join
                                 .Where(joined => joined.sa.TeacherId == staffTempId)
                                 .Select(joined => new TeacherSubjectAllocationViewModel
                                 {
                                     ClassId = joined.sa.ClassId ?? Guid.Empty,
                                     ClassName = joined.sa.Class.ClassName,
                                     SectionId = joined.sa.SectionId ?? Guid.Empty,
                                     SectionName = joined.sa.Section.Name,
                                     SubjectId = joined.sa.SubjectId ?? Guid.Empty,
                                     SubjectName = joined.sa.Subject.SubjectName,
                                     SubjectTotalMarks = joined.sa.Subject.SubjectTotalMarks,
                                     ExamId = joined.el.Id,  // Fetch the ExamId
                                     ExamName = joined.el.ExamName, // Fetch the ExamName
                                    
                                 })
                                 .ToList();

            // Create a dropdown list of ExamNames with ExamId as the value
            ViewBag.ExamDropdown = model.Select(m => new SelectListItem
            {
                Value = m.ExamId.ToString(),
                Text = m.ExamName
            }).ToList();

            return View(model);
        }
        public IActionResult GetMarksData()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var staffTempId = _context.Users
                                      .Where(u => u.Id == userId)
                                      .Select(u => u.StaffTempId)
                                      .FirstOrDefault();

            if (staffTempId == null)
            {
                return NotFound("StaffTempId not found for the current user.");
            }

            var data = _context.Marks
                                .Include(m => m.Subject)
                                .Include(m => m.Student)
                                .Join(_context.SubjectAllocations,
                                      m => m.Subject,
                                      sa => sa.SubjectId,
                                      (m, sa) => new
                                      {
                                          sa.Class.ClassName,
                                          sa.Section.Name,
                                          sa.Subject.SubjectName,
                                          m.StudentId,
                                          StudentName = _context.Students
                                                        .Where(s => s.Id == m.StudentId)
                                                        .Select(s => s.FullName)
                                                        .FirstOrDefault(),
                                          m.TotalMarks,
                                          m.ObtainMarks,
                                          m.Percentage,
                                          m.Grade,
                                          m.CreatedOn
                                      })
                                .Where(x => x.ClassName != null) // Apply any additional filters if necessary
                                .ToList();

            return Json(data);
        }

        public IActionResult GetStudentsByClassAndSection(Guid classId, Guid sectionId)
        {
            var students = _context.Students
                                   .Where(s => s.ClassId == classId && s.SectionId == sectionId)
                                   .Select(s => new
                                   {
                                       id = s.Id,
                                       fullName = s.FullName
                                   })
                                   .ToList();
            return Json(students);
        }

        public IActionResult GetSubjectTotalMarks(Guid subjectId)
        {
            var subject = _context.SubjectModels
                                  .Where(s => s.Id == subjectId)
                                  .Select(s => new
                                  {
                                      totalMarks = s.SubjectTotalMarks
                                  })
                                  .FirstOrDefault();
            return Json(subject);
        }

        [HttpGet]
        public JsonResult GetSectionsByClassId(Guid classId)
        {
            var sections = _context.Sections
                                   .Where(s => s.ClassId == classId)
                                   .Select(s => new
                                   {
                                       s.Id,
                                       s.Name
                                   })
                                   .ToList();

            return Json(sections);
        }

        [HttpGet]
        //public JsonResult GetSubjectsBySectionId(Guid sectionId)
        //{
        //    var subjects = _context.SubjectModels
        //                           .Where(sm => sm.SectonId == sectionId)
        //                           .Select(sm => new
        //                           {
        //                               sm.Id,
        //                               sm.SubjectName
        //                           })
        //                           .ToList();

        //    return Json(subjects);
        //}

        [HttpGet]
        public JsonResult GetSectiondata(Guid classId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
        
                var sectiondata = _context.Sections.Where(x => x.ClassId == classId).Select(x => new
                {
                    Name = x.Name,
                    Id = x.Id,
                }).ToList();
                return Json(new { data = sectiondata });
            
        }




        public IActionResult Attendance()
        {
            ViewBag.Classlist = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
       
       
    }
}
