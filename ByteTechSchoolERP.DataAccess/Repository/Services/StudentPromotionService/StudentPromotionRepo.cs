using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.HRModels;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudentPromotion;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Students;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.StudentPromotionService
{
    public class StudentPromotionRepo : GenericRepository<PromoteStudentViewModel>, IPromoteStudent
    {
        private readonly ByteTechSchoolERPContext _context;

        public StudentPromotionRepo(ByteTechSchoolERPContext db) : base(db)
        {
            _context = db;
        }

        public async Task<ResponseModel> CreateStudentPromotion(PromoteStudentViewModel studentModel)
        {
            var responseModel = new ResponseModel();

            if (studentModel == null || studentModel.StudentIds == null || !studentModel.StudentIds.Any())
            {
                return new ResponseModel
                {
                    isSuccess = false,
                    Message = "No students selected for promotion."
                };
            }

            int studentsPromoted = 0;
            int errorsCount = 0;

            try
            {
                foreach (var studentId in studentModel.StudentIds)
                {
                    var student = await _context.Students.FindAsync(studentId);
                    if (student != null)
                    {
                        student.ClassId = studentModel.PromoteClassId;
                        student.SectionId = studentModel.PromoteSectionId;
                        _context.Entry(student).State = EntityState.Modified;

                        studentsPromoted++;
                    }
                    else
                    {
                        errorsCount++;
                    }
                }

                // Save changes once after the loop
                await _context.SaveChangesAsync();

                responseModel.isSuccess = studentsPromoted > 0;
                responseModel.Message = studentsPromoted > 0
                    ? $"{studentsPromoted} students promoted successfully."
                    : "No students were promoted.";

                if (errorsCount > 0)
                {
                    responseModel.Message += $" There were {errorsCount} errors during the promotion process.";
                }
            }
            catch (Exception ex)
            {
                // Log exception details (ex.Message)
                responseModel.isSuccess = false;
                responseModel.Message = "An unexpected error occurred while promoting students.";
            }

            return responseModel;
        }
    }
}
