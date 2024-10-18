using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudent;
using ByteTechSchoolERP.Models.ViewModels;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.Students;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.StudentService
{
    public class StudentAdmitRapo : GenericRepository<StudentViewModel>, IStudentAdmit
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StudentAdmitRapo(ByteTechSchoolERPContext db, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> CreateStudentAdmit(StudentViewModel std)
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
           
            var responseModel = new ResponseModel();
            Student stds = new Student();
            stds.FullName = std.FullName;
            stds.Surname = std.Surname;
            stds.Cast = std.Cast;
            stds.RelationWithParent = std.RelationWithParent;
            stds.RelationWithGuardian = std.RelationWithGuardian;
            stds.Gender = std.Gender;
            stds.DateOfBirth = std.DateOfBirth;
            stds.PlaceOfBirth = std.PlaceOfBirth;
            stds.ParentName = std.ParentName;
            stds.ParentCNIC = std.ParentCNIC;
            stds.ParentEmail = std.ParentEmail;
            stds.ParentContactNo = std.ParentContactNo;
            stds.ParentOtherNo = std.ParentOtherNo;
            stds.ParentOccupation = std.ParentOccupation;
            stds.ParentIncome = std.ParentIncome;
            stds.AdmissionDate = std.AdmissionDate;
            stds.Religion = std.Religion;
            stds.PreviousSchoolName = std.PreviousSchoolName;
            stds.PreviousObtainedMarks = std.PreviousObtainedMarks;
            stds.PreviousTotalMarks = std.PreviousTotalMarks;
            stds.PreviousClass = std.PreviousClass;
            stds.PreviousRemarks = std.PreviousRemarks;
            stds.Address = std.Address;
            stds.ClassId = std.ClassId;
            stds.SectionId = std.SectionId;
            stds.Shift = std.Shift;
            stds.GuardianName = std.GuardianName;
            stds.GuardianContactNo = std.GuardianContactNo;
            stds.GuardianOtherNo = std.GuardianOtherNo;
            stds.GuardianOccupation = std.GuardianOccupation;
            stds.GuardianIncome = std.GuardianIncome;
            stds.GuardianCNIC = std.GuardianCNIC;
            stds.GuardianEmail = std.GuardianEmail;
            stds.CampusId = std.CampusId;
            stds.UserId = currentUserId;


            var imageProperties = typeof(StudentViewModel).GetProperties().Where(p => p.PropertyType == typeof(IFormFile));
            foreach (var imageProperty in imageProperties)
            {
                var images = (IFormFile)imageProperty.GetValue(std);
                if (images != null)
                {
                    // Create the voucherImage folder if it doesn't exist
                    var ImageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "StudentImage");
                    if (!Directory.Exists(ImageFolder))
                    {
                        Directory.CreateDirectory(ImageFolder);
                    }

                    // Save the uploaded image
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(images.FileName);
                    var imagePathOnDisk = Path.Combine(ImageFolder, uniqueFileName);

                    using (var stream = new FileStream(imagePathOnDisk, FileMode.Create))
                    {
                        await images.CopyToAsync(stream);
                    }

                    // Associate the image filename with the corresponding property in the Student entity
                    if (imageProperty.Name == nameof(StudentViewModel.StudentProfileUrlPathMV))
                    {
                        stds.StudentProfileUrl = uniqueFileName;
                    }
                    else if (imageProperty.Name == nameof(StudentViewModel.GuardianProfileUrlPathMV))
                    {
                        stds.GuardianProfileUrl = uniqueFileName;
                    }
                    else if (imageProperty.Name == nameof(StudentViewModel.SchoolLeavingCertificateUrlPathMV))
                    {
                        stds.SchoolLeavingCertificateUrl = uniqueFileName;
                    }
                    else if (imageProperty.Name == nameof(StudentViewModel.StudentFormBUrlPathMV))
                    {
                        stds.StudentFormBUrl = uniqueFileName;
                    }
                    else if (imageProperty.Name == nameof(StudentViewModel.GuardianCNICFrontUrlPathMV))
                    {
                        stds.GuardianCNICFrontUrl = uniqueFileName;
                    }
                    else if (imageProperty.Name == nameof(StudentViewModel.GuardianCNICBackUrlPathMV))
                    {
                        stds.GuardianCNICBackUrl = uniqueFileName;
                    }
                    else if (imageProperty.Name == nameof(StudentViewModel.OtherDocumentsUrl1PathMV))
                    {
                        stds.OtherDocumentsUrl1 = uniqueFileName;
                    }
                    else if (imageProperty.Name == nameof(StudentViewModel.OtherDocumentsUrl2PathMV))
                    {
                        stds.OtherDocumentsUrl2 = uniqueFileName;
                    }




                }
            }

            _context.Students.Add(stds);
            await _context.SaveChangesAsync();
            responseModel.isSuccess = true;
            responseModel.Message = "Student Admit submitted successfully!";
            return responseModel;
        }



    }
}
