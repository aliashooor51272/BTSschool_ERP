using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ILesson;
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
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.LessonService
{
    public class Lessonrepo : GenericRepository<Lesson>,ILesson
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Lessonrepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> AddOrUpdateLessons(Lesson lesson)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                var existinglesson = await _context.Lessons
             .FirstOrDefaultAsync(s => s.Id == lesson.Id);

                if (existinglesson != null)
                {
                    // Update existing subject
                    existinglesson.ClassId = lesson.ClassId;
                    existinglesson.SubjectId = lesson.SubjectId;
                    existinglesson.SectionId = lesson.SectionId;
                    existinglesson.LessonName = lesson.LessonName;
                  
                    _context.Lessons.Update(existinglesson);
                    responseModel.Message = "Lesson updated successfully.";
                }
                else
                {
                    // Add new subject
                    var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    lesson.UserId = currentUserId;
                    await _context.Lessons.AddAsync(lesson);
                    responseModel.Message = "Lesson added successfully.";
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
        public async Task<ResponseModel> AddLessons(Lesson lesson)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                await _context.Lessons.AddAsync(lesson);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Lesson added successfully.";
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"Error occurred while adding the subject: {ex.Message}";
            }

            return responseModel;
        }

        public List<LessonListViewModel> GetLessonList(LessonListViewModel lessonListViewModel)
        {
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

                var lessons = _context.Lessons
                    .Where(l => l.UserId == currentUserId) // Filter by UserId
                    .Join(_context.Classes,
                          l => l.ClassId,
                          c => c.Id,
                          (l, c) => new { Lesson = l, Class = c })
                    .Join(_context.Sections,
                          lc => lc.Lesson.SectionId,
                          s => s.Id,
                          (lc, s) => new { lc.Lesson, lc.Class, Section = s })
                    .Join(_context.SubjectModels,
                          lcs => lcs.Lesson.SubjectId,
                          sub => sub.Id,
                          (lcs, sub) => new LessonListViewModel
                          {
                              Id = lcs.Lesson.Id,
                              ClassId = lcs.Lesson.ClassId,
                              SectionId = lcs.Lesson.SectionId,
                              SubjectId = lcs.Lesson.SubjectId,
                              LessonName = lcs.Lesson.LessonName,
                              ClassName = lcs.Class.ClassName,
                              SectionName = lcs.Section.Name,
                              SubjectName = sub.SubjectName
                          })
                    .ToList();

                return lessons;
            }
            catch (Exception ex)
            {
                // Handle exception, log it, or rethrow as needed
                throw new Exception($"Error occurred while retrieving the lesson list: {ex.Message}");
            }
        }

        public async Task<ResponseModel> DeleteLessonById(Guid id)
        {
            var responseModel = new ResponseModel();

            try
            {
                // Find the subject by ID
                var lesson = await _context.Lessons.FindAsync(id);
                if (lesson == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.Message = "Lesson not found.";
                    return responseModel;
                }

                // Remove the subject
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Lesson deleted successfully.";
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
