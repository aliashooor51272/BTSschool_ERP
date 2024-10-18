using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISection;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.SectionService
{
    public class SectionRepo : GenericRepository<Section>, ISection
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SectionRepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseModel> AddOrUpdateSection(Section section)
        {
            var responseModel = new ResponseModel();

            try
            {
                var existingClass = await _context.Sections.FirstOrDefaultAsync(c => c.Id == section.Id);
                if (existingClass != null)
                {
                    existingClass.ClassId = section.ClassId;
                    existingClass.Name = section.Name;
                    existingClass.Description = section.Description;

                    _context.Sections.Update(existingClass);
                    responseModel.Message = "Class updated successfully.";
                }
                else
                {
                    var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    section.UserId = currentUserId;
                    await _context.Sections.AddAsync(section);
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
        public List<SectionViewModel> GetSectionList()
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            // Check if the current user is in the SubAdmin table
            var isSubAdmin = _context.SubAdminn.Any(sa => sa.Id.ToString() == currentUserId);
            if (isSubAdmin != null)
            {
                return _context.Sections
                           .Where(s => s.UserId == currentUserId) // Filter by UserId
                           .Select(s => new SectionViewModel
                           {
                               Id = s.Id,
                               Class = s.Class,
                               Name = s.Name,
                               Description = s.Description
                           }).ToList();
            }
            else
            {
                return _context.Sections
                         
                          .Select(s => new SectionViewModel
                          {
                              Id = s.Id,
                              Class = s.Class,
                              Name = s.Name,
                              Description = s.Description
                          }).ToList();
            }
        }

        public async Task<ResponseModel> DeleteSectionById(Guid id)
        {
            var responseModel = new ResponseModel();

            try
            {
                var classToDelete = await _context.Sections.FindAsync(id);
                if (classToDelete == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.Message = "Section not found.";
                    return responseModel;
                }

                _context.Sections.Remove(classToDelete);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Section deleted successfully.";
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
