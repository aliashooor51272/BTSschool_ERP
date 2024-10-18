using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISubject;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Lesson;
using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.SubjectService
{
    public class SubjectRepo : GenericRepository<SubjectModel>, ISubjectAdd
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SubjectRepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> AddOrUpdateSubject(SubjectModel subjectModel)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                var existingSubject = await _context.SubjectModels
             .FirstOrDefaultAsync(s => s.Id == subjectModel.Id);

                if (existingSubject != null)
                {
                    // Update existing subject
                    existingSubject.SubjectCode = subjectModel.SubjectCode;
                    existingSubject.SubjectName = subjectModel.SubjectName;
                    existingSubject.SubjectTotalMarks = subjectModel.SubjectTotalMarks;

                    _context.SubjectModels.Update(existingSubject);
                    responseModel.Message = "Subject updated successfully.";
                }
                else
                {
                    // Add new subject
                    var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    subjectModel.UserId = currentUserId;
                    await _context.SubjectModels.AddAsync(subjectModel);
                    responseModel.Message = "Subject added successfully.";
                }

                await _context.SaveChangesAsync();
                responseModel.isSuccess = true;
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"Error occurred while adding or updating the subject: {ex.Message}";
            }

            return responseModel;
        }

        public List<SubjectViewModel> GetSubjectLists(SubjectModel subjectModel)
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            // Fetch subjects from the database filtered by UserId
            var subjects = _context.SubjectModels
                                   .Where(s => s.UserId == currentUserId) // Filter by UserId
                                   .ToList();

            // Map subjects to SubjectViewModel
            var subjectViewModels = subjects.Select(subject => new SubjectViewModel
            {
                Id = subject.Id,
                SubjectName = subject.SubjectName,
                SubjectCode = subject.SubjectCode,
                SubjectTotalMarks = subject.SubjectTotalMarks ?? 0 // Default to 0 if null
            }).ToList();

            return subjectViewModels;
        }


        public async Task<ResponseModel> DeleteSubjectById(Guid id)
        {
            var responseModel = new ResponseModel();

            try
            {
                // Find the subject by ID
                var subject = await _context.SubjectModels.FindAsync(id);
                if (subject == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.Message = "Subject not found.";
                    return responseModel;
                }

                // Remove the subject
               
                _context.SubjectModels.Remove(subject);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Subject deleted successfully.";
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"Error occurred while deleting the subject: {ex.Message}";
            }

            return responseModel;
        }
    }
}
