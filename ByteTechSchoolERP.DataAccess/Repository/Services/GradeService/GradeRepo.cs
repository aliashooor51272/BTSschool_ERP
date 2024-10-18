using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Exam;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.ClassService
{
    public class GradeRepo : GenericRepository<Grade>, IGrade
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GradeRepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> AddOrUpdateGrade(Grade grade)
        {
            var responseModel = new ResponseModel();

            try
            {
                var existingGrade = await _context.Grades.FirstOrDefaultAsync(g => g.Id == grade.Id);
                if (existingGrade != null)
                {
                    // Update existing grade
                    existingGrade.GradeTitle = grade.GradeTitle;
                    existingGrade.Description = grade.Description;
                    existingGrade.Grades = grade.Grades;
                    existingGrade.MaximumPercentage = grade.MaximumPercentage;
                    existingGrade.MinimumPercentage = grade.MinimumPercentage;
                    existingGrade.Remark = grade.Remark;

                    _context.Grades.Update(existingGrade);
                    responseModel.Message = "Grade updated successfully.";
                }
                else
                {
                    // Add new grade
                    await _context.Grades.AddAsync(grade);
                    responseModel.Message = "Grade added successfully.";
                }

                await _context.SaveChangesAsync();
                responseModel.isSuccess = true;
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"Error: {ex.Message}";
            }

            return responseModel;
        }

        public List<GradeViewModel> GetGradeList()
        {
            var gradesQuery = _context.Grades.AsQueryable();

            return gradesQuery
                .Select(g => new GradeViewModel
                {
                    Id = g.Id,
                    GradeTitle = g.GradeTitle,
                    Description = g.Description,
                    Grades = g.Grades,
                    MaximumPercentage = g.MaximumPercentage,
                    MinimumPercentage = g.MinimumPercentage,
                    Remark = g.Remark
                })
                .ToList();
        }

        public async Task<ResponseModel> DeleteGradeById(Guid id)
        {
            var responseModel = new ResponseModel();

            try
            {
                var gradeToDelete = await _context.Grades.FindAsync(id);
                if (gradeToDelete == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.Message = "Grade not found.";
                    return responseModel;
                }

                _context.Grades.Remove(gradeToDelete);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Grade deleted successfully.";
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"Error: {ex.Message}";
            }

            return responseModel;
        }
    }
}
