using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClasstimetable;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.SubAdmin;
using ByteTechSchoolERP.Models.TimeTable;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.ClassService
{
    public class ClassTimetableRepo : GenericRepository<ClassTimetable>, IClasstimetable
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassTimetableRepo(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> SaveTimetableAsync(ClassTimetableViewModel model)
        {
            var response = new ResponseModel();

            try
            {
                // Create the ClassTimetable entity from the ViewModel
                var classTimetable = new ClassTimetable
                {
                    Id = Guid.NewGuid(),
                    Day = model.Day,
                    ClassId = model.ClassId,
                    SectionId = model.SectionId,
                    timetableEntries = model.TimetableEntries.Select(te => new TimetableEntries
                    {
                        Id = Guid.NewGuid(),
                        SubjectId = te.SubjectId,
                        StaffTempId = te.StaffTempId,
                        TimeFrom = te.TimeFrom,
                        TimeTo = te.TimeTo,
                        RoomNo = te.RoomNo
                    }).ToList()
                };

                // Add the ClassTimetable entity to the database
                _context.ClassTimetables.Add(classTimetable);
                await _context.SaveChangesAsync();

                response.isSuccess = true;
                response.Message = "Timetable saved successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here, but you should do it in a real application)
                response.isSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }
    }
}

