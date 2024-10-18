using Azure.Core;
using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Students;
using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class StudentManagementController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StudentManagementController(ByteTechSchoolERPContext context, IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult StudentPromotion()
        {   // Get all classes to populate the Class dropdown
            var classes = _context.Classes.ToList();
            ViewBag.Classes = new SelectList(classes, "Id", "ClassName");


            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> StudentInformation()
        {
            ViewBag.Classess = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
            ViewBag.Sections = new SelectList(_context.Sections.Select(x => new { Id = x.Id, Name = x.Name }), "Id", "Name"); // Corrected from ViewBag.Section to ViewBag.Sections
            return View();
        }
        [HttpGet]
        public JsonResult GetSectiondata(Guid classId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var isSubAdmin = _context.SubAdminn.Any(sa => sa.Id.ToString() == currentUserId);

            var sectiondata = _context.Sections
                .Where(x => x.ClassId == classId && (isSubAdmin ? x.UserId == currentUserId : true))
                .Select(x => new
                {
                    Name = x.Name,
                    Id = x.Id
                }).ToList();

            return Json(new { data = sectiondata });
        }
        [HttpGet]
        public JsonResult GetStudentdata()
        {
            var data = _context.Students.Include(x => x.Section).ThenInclude(x => x.Class).ToList();
            var StdData = data.Select(l => new
            {
                l.Id,
                l.FullName,
                l.ParentName,
               
                ClassId = l.Section.Class.Id,
                SectionId = l.Section.Id
            });
            return Json(new { data = StdData });
        }
       

        public async Task<IActionResult> EditInformation(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(); // Handle the case where the student is not found
            }

            // Map the student data to the view model
            var model = new StudentViewModel
            {
                Id = student.Id,
                FullName = student.FullName,
                Surname = student.Surname,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                PlaceOfBirth = student.PlaceOfBirth,
                ParentName = student.ParentName,
                ParentEmail = student.ParentEmail,
                ParentCNIC = student.ParentCNIC,
                ParentContactNo = student.ParentContactNo,
                ParentIncome = student.ParentIncome,
                ParentOccupation = student.ParentOccupation,
                ParentOtherNo = student.ParentOtherNo,
                Cast = student.Cast,
                PreviousClass = student.PreviousClass,
                PreviousObtainedMarks = student.PreviousObtainedMarks,
                PreviousRemarks = student.PreviousRemarks,
                ClassId = student.ClassId,
                PreviousSchoolName = student.PreviousSchoolName,
                PreviousTotalMarks = student.PreviousTotalMarks,
                SectionId = student.SectionId,
                Shift = student.Shift,
                AdmissionDate = student.AdmissionDate,
                Address = student.Address,
                Religion = student.Religion,
                GuardianName = student.GuardianName,
                GuardianCNIC = student.GuardianCNIC,
                GuardianContactNo = student.GuardianContactNo,
                GuardianEmail = student.GuardianEmail,
                GuardianIncome = student.GuardianIncome,
                GuardianOccupation = student.GuardianOccupation,
                GuardianOtherNo = student.GuardianOtherNo,

                // Populate existing image URLs
                StudentProfileUrl = student.StudentProfileUrl,
                GuardianProfileUrl = student.GuardianProfileUrl,
                SchoolLeavingCertificateUrl = student.SchoolLeavingCertificateUrl,
                StudentFormBUrl = student.StudentFormBUrl,
                GuardianCNICFrontUrl = student.GuardianCNICFrontUrl,
                GuardianCNICBackUrl = student.GuardianCNICBackUrl,
                OtherDocumentsUrl1 = student.OtherDocumentsUrl1,
                OtherDocumentsUrl2 = student.OtherDocumentsUrl2
            };

            ViewBag.Classess = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
            ViewBag.Sections = new SelectList(_context.Sections.Select(x => new { Id = x.Id, Name = x.Name }), "Id", "Name");

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditInformation(StudentViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Classess = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
            //    ViewBag.Sections = new SelectList(_context.Sections.Select(x => new { Id = x.Id, Name = x.Name }), "Id", "Name");
            //    return View(model);
            //}
            var student = await _context.Students.FindAsync(model.Id);

            if (student == null)
            {
                return NotFound(); // Handle the case where the student is not found
            }
            else
            {
                // Update the student's properties (excluding image URLs)
                student.FullName = model.FullName;
                student.Surname = model.Surname;
                student.Gender = model.Gender;
                student.DateOfBirth = model.DateOfBirth;
                student.PlaceOfBirth = model.PlaceOfBirth;
                student.ParentName = model.ParentName;
                student.ParentEmail = model.ParentEmail;
                student.ParentCNIC = model.ParentCNIC;
                student.ParentContactNo = model.ParentContactNo;
                student.ParentIncome = model.ParentIncome;
                student.ParentOccupation = model.ParentOccupation;
                student.ParentOtherNo = model.ParentOtherNo;
                student.Cast = model.Cast;
                student.PreviousClass = model.PreviousClass;
                student.PreviousObtainedMarks = model.PreviousObtainedMarks;
                student.PreviousRemarks = model.PreviousRemarks;
                student.ClassId = model.ClassId;
                student.PreviousSchoolName = model.PreviousSchoolName;
                student.PreviousTotalMarks = model.PreviousTotalMarks;
                student.SectionId = model.SectionId;
                student.Shift = model.Shift;
                student.AdmissionDate = model.AdmissionDate;
                student.Address = model.Address;
                student.Religion = model.Religion;
                student.GuardianName = model.GuardianName;
                student.GuardianCNIC = model.GuardianCNIC;
                student.GuardianContactNo = model.GuardianContactNo;
                student.GuardianEmail = model.GuardianEmail;
                student.GuardianIncome = model.GuardianIncome;
                student.GuardianOccupation = model.GuardianOccupation;
                student.GuardianOtherNo = model.GuardianOtherNo;

                // Handle image uploads
                var imageProperties = typeof(StudentViewModel).GetProperties().Where(p => p.PropertyType == typeof(IFormFile));
                foreach (var imageProperty in imageProperties)
                {
                    var imageFile = (IFormFile)imageProperty.GetValue(model); // Get the IFormFile from the model
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Ensure the image folder exists
                        var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "StudentImage");
                        if (!Directory.Exists(imageFolder))
                        {
                            Directory.CreateDirectory(imageFolder);
                        }

                        // Generate a unique file name and save the image
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var imagePathOnDisk = Path.Combine(imageFolder, uniqueFileName);

                        using (var stream = new FileStream(imagePathOnDisk, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // Update the corresponding image path in the student entity
                        switch (imageProperty.Name)
                        {
                            case nameof(StudentViewModel.StudentProfileUrlPathMV):
                                student.StudentProfileUrl = uniqueFileName;
                                break;
                            case nameof(StudentViewModel.GuardianProfileUrlPathMV):
                                student.GuardianProfileUrl = uniqueFileName;
                                break;
                            case nameof(StudentViewModel.SchoolLeavingCertificateUrlPathMV):
                                student.SchoolLeavingCertificateUrl = uniqueFileName;
                                break;
                            case nameof(StudentViewModel.StudentFormBUrlPathMV):
                                student.StudentFormBUrl = uniqueFileName;
                                break;
                            case nameof(StudentViewModel.GuardianCNICFrontUrlPathMV):
                                student.GuardianCNICFrontUrl = uniqueFileName;
                                break;
                            case nameof(StudentViewModel.GuardianCNICBackUrlPathMV):
                                student.GuardianCNICBackUrl = uniqueFileName;
                                break;
                            case nameof(StudentViewModel.OtherDocumentsUrl1PathMV):
                                student.OtherDocumentsUrl1 = uniqueFileName;
                                break;
                            case nameof(StudentViewModel.OtherDocumentsUrl2PathMV):
                                student.OtherDocumentsUrl2 = uniqueFileName;
                                break;
                        }
                    }
                    else
                    {
                        // If no new file is provided, keep the existing image URL
                        switch (imageProperty.Name)
                        {
                            case nameof(StudentViewModel.StudentProfileUrlPathMV):
                                if (string.IsNullOrEmpty(student.StudentProfileUrl))
                                    student.StudentProfileUrl = model.StudentProfileUrl;
                                break;
                            case nameof(StudentViewModel.GuardianProfileUrlPathMV):
                                if (string.IsNullOrEmpty(student.GuardianProfileUrl))
                                    student.GuardianProfileUrl = model.GuardianProfileUrl;
                                break;
                            case nameof(StudentViewModel.SchoolLeavingCertificateUrlPathMV):
                                if(string.IsNullOrEmpty(student.SchoolLeavingCertificateUrl))
                                    student.SchoolLeavingCertificateUrl = model.SchoolLeavingCertificateUrl;
                                break;
                            case nameof(StudentViewModel.StudentFormBUrlPathMV):
                                if(string.IsNullOrEmpty(student.StudentFormBUrl))
                                    student.StudentFormBUrl = model.StudentFormBUrl;
                            break;
                            case nameof(StudentViewModel.GuardianCNICFrontUrlPathMV):
                                if(string.IsNullOrEmpty(student.GuardianCNICFrontUrl))
                                    student.GuardianCNICFrontUrl = model.GuardianCNICFrontUrl;
                            break;
                            case nameof(StudentViewModel.GuardianCNICBackUrlPathMV):
                                if (string.IsNullOrEmpty(student.GuardianCNICBackUrl))
                                    student.GuardianCNICBackUrl = model.GuardianCNICBackUrl;
                            break;
                         
                            case nameof(StudentViewModel.OtherDocumentsUrl1PathMV):
                                if(string.IsNullOrEmpty(student.OtherDocumentsUrl1))
                                student.OtherDocumentsUrl1 =model.OtherDocumentsUrl1;
                                break;
                            case nameof(StudentViewModel.OtherDocumentsUrl2PathMV):
                                if (string.IsNullOrEmpty(student.OtherDocumentsUrl2))
                                    student.OtherDocumentsUrl2 = model.OtherDocumentsUrl2;
                                break;
                        }
                    }
                }

                // Save changes to the database
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }

            

            // Redirect to StudentInformation action after successful update
            return RedirectToAction("StudentInformation", "StudentManagement");
        }


        [HttpPost]
        public async Task<IActionResult> PromoteStudents([FromBody] PromoteStudentViewModel request)
        {
            var response = await _unitOfWork.Student.CreateStudentPromotion(request);

            if (!response.isSuccess)
            {
                return BadRequest(response.Message);
            }

            TempData["PromotionResult"] = response.Message;
            return RedirectToAction("StudentPromotion", "StudentManagement");
        }
        public IActionResult StudentTransfer()
        {
            // Get all classes to populate the Class dropdown
            var classes = _context.Classes.ToList();
            ViewBag.Classes = new SelectList(classes, "Id", "ClassName");

            // Get all campuses to populate the Campus dropdown
            var campuses = _context.SchoolCampus.ToList();
            ViewBag.Campuses = new SelectList(campuses, "CampusId", "CampusName");

            // Initialize empty lists for Sections and Students
            ViewBag.Sections = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            ViewBag.Students = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");

            return View();
        }

        public IActionResult GetSectionsByClassId(Guid classId)
        {
            var sections = _context.Sections.Where(s => s.ClassId == classId).ToList();
            return Json(sections.Select(s => new { id = s.Id, name = s.Name }));
        }

        public IActionResult GetStudentsBySectionId(Guid sectionId)
        {
            var students = _context.Students.Where(s => s.SectionId == sectionId).ToList();
            return Json(students.Select(s => new { id = s.Id, fullName = s.FullName }));
        }

        public IActionResult TransferStudent(Guid studentId, string campusId)
        {
            // Get the student by ID
            var student = _context.Students.Find(studentId);

            if (student != null)
            {
                // Update the student's campus ID
                student.CampusId = campusId;
                _context.SaveChanges();
            }

            return RedirectToAction("StudentTransfer");
        }
        public async Task<IActionResult> StudentReport()
        {
            return View();
        }
        public async Task<IActionResult> StudentAttendence()
        {
            ViewBag.Classlist = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SubmitAttendance([FromBody] List<StudentAttendanceVM> attendanceData)
        {
            if (attendanceData == null || attendanceData.Count == 0)
            {
                return BadRequest("Attendance data is required.");
            }

            var response = await _unitOfWork.StudentAttendance.MarkStudentAttendance(attendanceData);

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = response.Message;
                return Json(new { isSuccess = true, message = response.Message });
            }
            else
            {
                return Json(new { isSuccess = false, message = "Validation failed." });
            }


            return RedirectToAction("StudentAttendance", "StudentManagement");
        }
        [HttpGet]
        public JsonResult GetFilteredStudentData(Guid? classId, Guid? sectionId)
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            // Get today's date
            var today = DateTime.Today;

            // Query to get students and filter out those who have attendance for today
            var data = _context.Students
                .Include(s => s.Class)   // Eager load the Class entity
                .Include(s => s.Section) // Eager load the Section entity
                .Where(x => x.ClassId == classId && x.SectionId == sectionId)
                .Where(x => !_context.StudentAttendances
                    .Any(a => a.StudentId == x.Id && a.AttendanceDate == today))
                .ToList();

            var StdData = data.Select(l => new
            {
                l.Id,
                l.FullName,
                l.ParentName,
                l.ParentCNIC,
                l.ParentOccupation,
                l.ParentContactNo,
                ClassName = l.Class?.ClassName,
                SectionName = l.Section?.Name,
                l.DateOfBirth,
                l.StudentProfileUrl

            }).ToList();

            return Json(new { data = StdData });
        }

        
    }
}

