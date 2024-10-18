using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.SubAdmin;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.ClassService
{
    public class ClassRepo : GenericRepository<Class>, IClass
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassRepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> AddOrUpdateClasses(Class classes)
        {
            var responseModel = new ResponseModel();

            try
            {
                var existingClass = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classes.Id);
                if (existingClass != null)
                {
                    existingClass.ClassName = classes.ClassName;
                    existingClass.Description = classes.Description;

                    _context.Classes.Update(existingClass);
                    responseModel.Message = "Class updated successfully.";
                }
                else
                {
                    var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    classes.UserId = currentUserId;
                    await _context.Classes.AddAsync(classes);
                    responseModel.Message = "Class added successfully.";
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

        public List<ClassesViewModel> GetClassList()
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            // Check if the current user is in the SubAdmin table
            var isSubAdmin = _context.SubAdminn.Any(sa => sa.Id.ToString() == currentUserId);
            if (isSubAdmin != null)
            {
                return _context.Classes
                           .Where(c => c.UserId == currentUserId) // Filter by UserId
                           .Select(c => new ClassesViewModel
                           {
                               Id = c.Id,
                               ClassName = c.ClassName,
                               Description = c.Description
                           }).ToList();
            }
            else
            {
                return _context.Classes
                         
                          .Select(c => new ClassesViewModel
                          {
                              Id = c.Id,
                              ClassName = c.ClassName,
                              Description = c.Description
                          }).ToList();
            }
        }

        public async Task<ResponseModel> DeleteClassById(Guid id)
        {
            var responseModel = new ResponseModel();

            try
            {
                var classToDelete = await _context.Classes.FindAsync(id);
                if (classToDelete == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.Message = "Class not found.";
                    return responseModel;
                }

                _context.Classes.Remove(classToDelete);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Class deleted successfully.";
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
