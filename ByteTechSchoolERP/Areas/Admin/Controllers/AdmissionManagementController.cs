using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.Models.Students;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork; // Add this using statement

using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ByteTechSchoolERP.Models.HR;
using System.Security.Claims;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class AdmissionManagementController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AdmissionManagementController(ByteTechSchoolERPContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<IActionResult> AdmitStudent()
        {
            
            ViewBag.Clasess = new SelectList(_context.Classes.Select(x => new { Id = x.Id, Name = x.ClassName }), "Id", "Name");
            ViewBag.Campus = new SelectList(_context.SchoolCampus.Select(x => new { Id = x.CampusId, Name = x.CampusName }), "Id", "Name");
            // ViewBag.Classes = await _context.Classes.ToListAsync();
            ViewBag.Sections = await _context.Sections.ToListAsync();

            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            List<string> fileColumns = new List<string>();
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            fileColumns.Add(worksheet.Cells[1, col].Text);
                        }
                    }
                }
            }

            return Ok(fileColumns);
        }
        [HttpPost]
        public async Task<IActionResult> SaveFileData(IFormFile file, string mappings)
        {
            if (file == null || string.IsNullOrEmpty(mappings))
            {
                return BadRequest("File or mappings not provided");
            }

            var mappingDict = JsonConvert.DeserializeObject<List<Mapping>>(mappings);
            var students = new List<Student>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        return BadRequest("No worksheet found in the file");
                    }

                    // Get the header row
                    var headers = new Dictionary<string, int>();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        headers[worksheet.Cells[1, col].Text] = col;
                    }

                    // Iterate through each row (starting from row 2 to skip the header)
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var student = new Student();
                        foreach (var mapping in mappingDict)
                        {
                            if (headers.TryGetValue(mapping.FileColumn, out int colIndex))
                            {
                                var cellValue = worksheet.Cells[row, colIndex].Text;

                                // Map the cell value to the student property
                                switch (mapping.DbColumn)
                                {
                                    case "FullName":
                                        student.FullName = cellValue;
                                        break;
                                    case "Surname":
                                        student.Surname = cellValue;
                                        break;
                                    case "Gender":
                                        student.Gender = cellValue;
                                        break;
                                    case "Cast":
                                        student.Cast = cellValue;
                                        break;
                                    case "RelationWithParent":
                                        student.RelationWithParent = cellValue;
                                        break;
                                    case "RelationWithGuardian":
                                        student.RelationWithGuardian = cellValue;
                                        break;
                                   
                                    case "PlaceOfBirth":
                                        student.PlaceOfBirth = cellValue;
                                        break;
                                    case "ParentName":
                                        student.ParentName = cellValue;
                                        break;
                                    case "ParentCNIC":
                                        student.ParentCNIC = cellValue;
                                        break;
                                    case "ParentEmail":
                                        student.ParentEmail = cellValue;
                                        break;
                                    case "ParentContactNo":
                                        student.ParentContactNo = cellValue;
                                        break;
                                    case "ParentOtherNo":
                                        student.ParentOtherNo = cellValue;
                                        break;
                                    case "ParentOccupation":
                                        student.ParentOccupation = cellValue;
                                        break;
                                    case "ParentIncome":
                                        student.ParentIncome = decimal.TryParse(cellValue, out var parentIncome) ? parentIncome : (decimal?)null;
                                        break;
                                    case "AdmissionDate":
                                        student.AdmissionDate = DateTime.TryParse(cellValue, out var admissionDate) ? admissionDate : (DateTime?)null;
                                        break;
                                    case "Religion":
                                        student.Religion = cellValue;
                                        break;
                                    case "PreviousSchoolName":
                                        student.PreviousSchoolName = cellValue;
                                        break;
                                    case "PreviousObtainedMarks":
                                        student.PreviousObtainedMarks = decimal.TryParse(cellValue, out var obtainedMarks) ? obtainedMarks : (decimal?)null;
                                        break;
                                    case "PreviousTotalMarks":
                                        student.PreviousTotalMarks = decimal.TryParse(cellValue, out var totalMarks) ? totalMarks : (decimal?)null;
                                        break;
                                    case "PreviousClass":
                                        student.PreviousClass = cellValue;
                                        break;
                                    case "PreviousRemarks":
                                        student.PreviousRemarks = cellValue;
                                        break;
                                    case "Address":
                                        student.Address = cellValue;
                                        break;
                                    case "ClassId":
                                        student.ClassId = Guid.TryParse(cellValue, out var classId) ? classId : (Guid?)null;
                                        break;
                                    case "SectionId":
                                        student.SectionId = Guid.TryParse(cellValue, out var sectionId) ? sectionId : (Guid?)null;
                                        break;
                                    case "Shift":
                                        student.Shift = cellValue;
                                        break;
                                    case "GuardianName":
                                        student.GuardianName = cellValue;
                                        break;
                                    case "GuardianContactNo":
                                        student.GuardianContactNo = cellValue;
                                        break;
                                    case "GuardianOtherNo":
                                        student.GuardianOtherNo = cellValue;
                                        break;
                                    case "GuardianOccupation":
                                        student.GuardianOccupation = cellValue;
                                        break;
                                    case "GuardianIncome":
                                        student.GuardianIncome = decimal.TryParse(cellValue, out var guardianIncome) ? guardianIncome : (decimal?)null;
                                        break;
                                    case "GuardianCNIC":
                                        student.GuardianCNIC = cellValue;
                                        break;
                                    case "GuardianEmail":
                                        student.GuardianEmail = cellValue;
                                        break;
                                    case "StudentProfileUrl":
                                        student.StudentProfileUrl = cellValue;
                                        break;
                                    case "GuardianProfileUrl":
                                        student.GuardianProfileUrl = cellValue;
                                        break;
                                    case "SchoolLeavingCertificateUrl":
                                        student.SchoolLeavingCertificateUrl = cellValue;
                                        break;
                                    case "StudentFormBUrl":
                                        student.StudentFormBUrl = cellValue;
                                        break;
                                    case "GuardianCNICFrontUrl":
                                        student.GuardianCNICFrontUrl = cellValue;
                                        break;
                                    case "GuardianCNICBackUrl":
                                        student.GuardianCNICBackUrl = cellValue;
                                        break;
                                    case "OtherDocumentsUrl1":
                                        student.OtherDocumentsUrl1 = cellValue;
                                        break;
                                    case "OtherDocumentsTitle1":
                                        student.OtherDocumentsTitle1 = cellValue;
                                        break;
                                    case "OtherDocumentsUrl2":
                                        student.OtherDocumentsUrl2 = cellValue;
                                        break;
                                    case "OtherDocumentsTitle2":
                                        student.OtherDocumentsTitle2 = cellValue;
                                        break;
                                }
                            }
                        }
                        students.Add(student);
                    }
                }
            }

            _context.Students.AddRange(students);
            await _context.SaveChangesAsync();

            return Ok("Data saved successfully");
        }
    

    public class Mapping
    {
        public string FileColumn { get; set; }
        public string DbColumn { get; set; }
    }
    [HttpPost]
        public async Task<IActionResult> AdmitStudents(StudentViewModel std)
        {
            if (std == null)
            {
                return BadRequest("Student data is required.");
            }

            var response = await _unitOfWork.StudentAdmit.CreateStudentAdmit(std);

            if (response == null)
            {
                return Json(new { isSuccess = false, message = "An error occurred while processing your request." });
            }

            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = response.Message ?? "Operation successful.";
                //return Json(new { isSuccess = true, message = response.Message });
                return RedirectToAction("StudentInformation", "StudentManagement");
            }
            else
            {
                return Json(new { isSuccess = false, message = response.Message ?? "Validation failed." });
            }

            // The following line will never be reached since the method already returns a response in all cases
            // return RedirectToAction("StudentAttendance", "StudentManagement");
        }



        [HttpGet]
        public JsonResult GetTheRelatedSection(Guid classId)
        {
            var SectionData = _context.Sections
                .Where(x => x.ClassId == classId)
                .Select(x => new
                {
                    Name = x.Name,
                    Id = x.Id,

                })
                .ToList(); // Ensure only one object is returned
            return Json(new { data = SectionData });
        }

        
        public IActionResult Index()
        {
            return View();
        }
         
        private async Task<string> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Example: Save image to a folder and return the URL
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/images/" + uniqueFileName; // Example URL
        }

        private async Task<string> SaveDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Example: Save document to a folder and return the URL
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/documents/" + uniqueFileName; // Example URL
        }


        
        public class MappedColumn
        {
            public string FileColumn { get; set; }
            public string DatabaseColumn { get; set; }
        }

        public IActionResult UploadStudentExcel()
        {
            var classes = _context.Classes

         .Select(c => new SelectListItem
         {
             Text = c.ClassName,
             Value = c.Id.ToString()
         }).ToList();

          

            ViewBag.Classes = new SelectList(classes, "Value", "Text");
 
            return View();
        }
        // Controller Method to Get Sections based on ClassId
        [HttpGet]
       
        public IActionResult GetSectionsByClass(Guid classId)
        {
            var sections = _context.Sections
                .Where(s => s.ClassId == classId)
                .Select(s => new
                {
                    value = s.Id.ToString(),
                    text = s.Name
                })
                .ToList();

            return Json(sections);
        }


        public async Task<IActionResult> AdmissionRequest()
        {
            return View();
        }

        public async Task<IActionResult> AdmissionInquries()
        {
            return View();
        }
         public async Task<IActionResult> AdmitBulkStudent()
        {
            return View();
        }

        public async Task<IActionResult> AdmissionReports()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadStudentExcel(IFormCollection form , Guid SectionId , Guid ClassId)
            {

            var mappedColumns = JsonConvert.DeserializeObject<List<MappedColumn>>(form["mappedColumns"]);
            var fileData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(form["fileData"]);

            foreach (var row in fileData)
            {
                var student = new Student();
                foreach (var mappedColumn in mappedColumns)
                {
                    student.ClassId = ClassId;
                    student.SectionId = SectionId;
                    if (row.ContainsKey(mappedColumn.FileColumn))
                    {
                        switch (mappedColumn.DatabaseColumn)
                        {
                            case "FullName":
                                student.FullName = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "Surname":
                                student.Surname = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "Cast":
                                student.Cast = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "RelationWithParent":
                                student.RelationWithParent = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "RelationWithGuardian":
                                student.RelationWithGuardian = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "Gender":
                                student.Gender = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "DateOfBirth":
                                if (DateTime.TryParse(row[mappedColumn.FileColumn]?.ToString(), out var dob))
                                {
                                    student.DateOfBirth = dob;
                                }
                                break;
                            case "PlaceOfBirth":
                                student.PlaceOfBirth = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "ParentName":
                                student.ParentName = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "ParentCNIC":
                                student.ParentCNIC = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "ParentEmail":
                                student.ParentEmail = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "ParentContactNo":
                                student.ParentContactNo = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "ParentOtherNo":
                                student.ParentOtherNo = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "ParentOccupation":
                                student.ParentOccupation = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "ParentIncome":
                                if (decimal.TryParse(row[mappedColumn.FileColumn]?.ToString(), out var parentIncome))
                                {
                                    student.ParentIncome = parentIncome;
                                }
                                break;
                            case "AdmissionDate":
                                if (DateTime.TryParse(row[mappedColumn.FileColumn]?.ToString(), out var admissionDate))
                                {
                                    student.AdmissionDate = admissionDate;
                                }
                                break;
                            case "Religion":
                                student.Religion = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "PreviousSchoolName":
                                student.PreviousSchoolName = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "PreviousObtainedMarks":
                                if (decimal.TryParse(row[mappedColumn.FileColumn]?.ToString(), out var obtainedMarks))
                                {
                                    student.PreviousObtainedMarks = obtainedMarks;
                                }
                                break;
                            case "PreviousTotalMarks":
                                if (decimal.TryParse(row[mappedColumn.FileColumn]?.ToString(), out var totalMarks))
                                {
                                    student.PreviousTotalMarks = totalMarks;
                                }
                                break;
                            case "PreviousClass":
                                student.PreviousClass = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "PreviousRemarks":
                                student.PreviousRemarks = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "Address":
                                student.Address = row[mappedColumn.FileColumn]?.ToString();
                                break;
                       
                            case "ParentId":
                                if (Guid.TryParse(row[mappedColumn.FileColumn]?.ToString(), out var parentId))
                                {
                                    student.ParentId = parentId;
                                }
                                break;
                       
                            case "Shift":
                                student.Shift = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianName":
                                student.GuardianName = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianContactNo":
                                student.GuardianContactNo = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianOtherNo":
                                student.GuardianOtherNo = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianOccupation":
                                student.GuardianOccupation = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianIncome":
                                if (decimal.TryParse(row[mappedColumn.FileColumn]?.ToString(), out var guardianIncome))
                                {
                                    student.GuardianIncome = guardianIncome;
                                }
                                break;
                            case "GuardianCNIC":
                                student.GuardianCNIC = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianEmail":
                                student.GuardianEmail = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "StudentProfileUrl":
                                student.StudentProfileUrl = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianProfileUrl":
                                student.GuardianProfileUrl = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "SchoolLeavingCertificateUrl":
                                student.SchoolLeavingCertificateUrl = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "StudentFormBUrl":
                                student.StudentFormBUrl = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianCNICFrontUrl":
                                student.GuardianCNICFrontUrl = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "GuardianCNICBackUrl":
                                student.GuardianCNICBackUrl = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "OtherDocumentsUrl1":
                                student.OtherDocumentsUrl1 = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "OtherDocumentsTitle1":
                                student.OtherDocumentsTitle1 = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "OtherDocumentsUrl2":
                                student.OtherDocumentsUrl2 = row[mappedColumn.FileColumn]?.ToString();
                                break;
                            case "OtherDocumentsTitle2":
                                student.OtherDocumentsTitle2 = row[mappedColumn.FileColumn]?.ToString();
                                break;
                        }
                    }
                }

                _context.Students.Add(student);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("UploadStudentExcel");
        }

    }
}
