using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.ClassAndSection;
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
    public class TermRepo : GenericRepository<Term>, ITerm
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TermRepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> AddOrUpdateTerm(Term term)
        {
            var responseModel = new ResponseModel();

            try
            {
                var existingTerm = await _context.Terms.FirstOrDefaultAsync(c => c.Id == term.Id);
                if (existingTerm != null)
                {
                    existingTerm.Name = term.Name;
                    existingTerm.Description = term.Description;

                    _context.Terms.Update(existingTerm);
                    responseModel.Message = "Term updated successfully.";
                }
                else
                {
                    var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    term.UserId = currentUserId;
                    await _context.Terms.AddAsync(term);
                    responseModel.Message = "Term added successfully.";
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

        public List<TermViewModel> GetTermList()
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var isSubAdmin = _context.SubAdminn.Any(sa => sa.Id.ToString() == currentUserId);

            var termsQuery = _context.Terms.AsQueryable();

            if (isSubAdmin)
            {
                termsQuery = termsQuery.Where(c => c.UserId == currentUserId);
            }

            return termsQuery
                .Select(c => new TermViewModel
                {
                    Id = c.Id,
                    Name = c.Name, // Ensure this property is included
                    Description = c.Description
                })
                .ToList();
        }

        public async Task<ResponseModel> DeleteTermById(Guid id)
        {
            var responseModel = new ResponseModel();

            try
            {
                var termToDelete = await _context.Terms.FindAsync(id);
                if (termToDelete == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.Message = "Term not found.";
                    return responseModel;
                }

                _context.Terms.Remove(termToDelete);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Term deleted successfully.";
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
