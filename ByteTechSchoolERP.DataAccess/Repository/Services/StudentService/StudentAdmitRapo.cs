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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ByteTechSchoolERP.Models.HR;
using System.Data;

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

            // Validate password and confirm password

            var parentRole = await _context.Roles
                               .Where(r => r.Name == "Parent")
                               .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                               .FirstOrDefaultAsync();
            // Now, save the guardian's information to the Employee table
            var guardianEmail = std.GuardianEmail;
            var guardianPhone = std.GuardianContactNo;
            var employee = new Employee
            {
                FirstName = std.GuardianName, // You might want to adjust this
                Email = guardianEmail,
                Phone = guardianPhone,
                Password = "Parent123@#$", // This should be hashed before saving
                RoleId = parentRole.Id,// Assign the role ID
                
            };
            // Hash the password if you're using Identity
            

            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
            var guardianId = await _context.Employee
    .Where(e => e.Email == std.GuardianEmail)
    .Select(e => e.EmployeeId)
    .FirstOrDefaultAsync();
            // Create the Student entity
            Student stds = new Student
            {
                FullName = std.FullName,
                Surname = std.Surname,
                Cast = std.Cast,
                ParentId = guardianId,
                RelationWithParent = std.RelationWithParent,
                RelationWithGuardian = std.RelationWithGuardian,
                Gender = std.Gender,
                DateOfBirth = std.DateOfBirth,
                PlaceOfBirth = std.PlaceOfBirth,
                ParentName = std.ParentName,
                ParentCNIC = std.ParentCNIC,
                ParentEmail = std.ParentEmail,
                ParentContactNo = std.ParentContactNo,
                ParentOtherNo = std.ParentOtherNo,
                ParentOccupation = std.ParentOccupation,
                ParentIncome = std.ParentIncome,
                AdmissionDate = std.AdmissionDate,
                Religion = std.Religion,
                PreviousSchoolName = std.PreviousSchoolName,
                PreviousObtainedMarks = std.PreviousObtainedMarks,
                PreviousTotalMarks = std.PreviousTotalMarks,
                PreviousClass = std.PreviousClass,
                PreviousRemarks = std.PreviousRemarks,
                Address = std.Address,
                ClassId = std.ClassId,
                SectionId = std.SectionId,
                Shift = std.Shift,
                GuardianName = std.GuardianName,
                GuardianContactNo = std.GuardianContactNo,
                GuardianOtherNo = std.GuardianOtherNo,
                GuardianOccupation = std.GuardianOccupation,
                GuardianIncome = std.GuardianIncome,
                GuardianCNIC = std.GuardianCNIC,
                GuardianEmail = std.GuardianEmail,
                CampusId = std.CampusId,
                UserId = currentUserId,
                StudentProfileUrl = std.StudentProfileUrl ?? string.Empty,
                GuardianProfileUrl = std.GuardianProfileUrl ?? string.Empty,
                SchoolLeavingCertificateUrl = std.SchoolLeavingCertificateUrl ?? string.Empty,
                StudentFormBUrl = std.StudentFormBUrl ?? string.Empty,
                GuardianCNICFrontUrl = std.GuardianCNICFrontUrl ?? string.Empty,
                GuardianCNICBackUrl = std.GuardianCNICBackUrl ?? string.Empty,
                OtherDocumentsUrl1 = std.OtherDocumentsUrl1 ?? string.Empty,
                OtherDocumentsUrl2 = std.OtherDocumentsUrl2 ?? string.Empty
            };

            // Handle file uploads
            var imageProperties = typeof(StudentViewModel).GetProperties()
                .Where(p => p.PropertyType == typeof(IFormFile));

            foreach (var imageProperty in imageProperties)
            {
                var file = (IFormFile)imageProperty.GetValue(std);
                if (file != null)
                {
                    // Define the folder path to save images
                    var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "StudentImage");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    // Generate a unique file name and save the uploaded file
                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var imagePathOnDisk = Path.Combine(imageFolder, uniqueFileName);

                    using (var stream = new FileStream(imagePathOnDisk, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Associate the uploaded file's filename with the respective Student property
                    switch (imageProperty.Name)
                    {
                        case nameof(StudentViewModel.StudentProfileUrlPathMV):
                            stds.StudentProfileUrl = uniqueFileName;
                            break;
                        case nameof(StudentViewModel.GuardianProfileUrlPathMV):
                            stds.GuardianProfileUrl = uniqueFileName;
                            break;
                        case nameof(StudentViewModel.SchoolLeavingCertificateUrlPathMV):
                            stds.SchoolLeavingCertificateUrl = uniqueFileName;
                            break;
                        case nameof(StudentViewModel.StudentFormBUrlPathMV):
                            stds.StudentFormBUrl = uniqueFileName;
                            break;
                        case nameof(StudentViewModel.GuardianCNICFrontUrlPathMV):
                            stds.GuardianCNICFrontUrl = uniqueFileName;
                            break;
                        case nameof(StudentViewModel.GuardianCNICBackUrlPathMV):
                            stds.GuardianCNICBackUrl = uniqueFileName;
                            break;
                        case nameof(StudentViewModel.OtherDocumentsUrl1PathMV):
                            stds.OtherDocumentsUrl1 = uniqueFileName;
                            break;
                        case nameof(StudentViewModel.OtherDocumentsUrl2PathMV):
                            stds.OtherDocumentsUrl2 = uniqueFileName;
                            break;
                    }
                }
            }

            // Save the student record to the database
            _context.Students.Add(stds);
            await _context.SaveChangesAsync();

            // Prepare the response model
            responseModel.isSuccess = true;
            responseModel.Message = "Student Admit submitted successfully!";
            return responseModel;
        }



    }
}
