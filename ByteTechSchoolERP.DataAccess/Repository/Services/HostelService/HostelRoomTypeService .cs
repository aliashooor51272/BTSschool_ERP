using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IHostel;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISubject;
using ByteTechSchoolERP.DataAccess.Repository.IRepository;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Hostel;
using ByteTechSchoolERP.Models.Subjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.HostelService
{
    public class HostelRoomTypeService : GenericRepository<SubjectModel>, IHostelRoomTypeService
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HostelRoomTypeService(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _context = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> AddHostelRoomType(HostelRoomType hostelRoomType)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                var existingRoomType = await _context.HostelRoomTypes
             .FirstOrDefaultAsync(s => s.Id == hostelRoomType.Id);

                if (existingRoomType != null)
                {
                    // Update existing subject
                    existingRoomType.HostelRoomTypeName = hostelRoomType.HostelRoomTypeName;
                    existingRoomType.HostelRoomTypeDescription = hostelRoomType.HostelRoomTypeDescription;
 
                    _context.HostelRoomTypes.Update(existingRoomType);
                    responseModel.Message = "Room Type updated successfully.";
                }
                else
                {
                    // Add new subject
                    var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    hostelRoomType.UserId = currentUserId;
                    await _context.HostelRoomTypes.AddAsync(hostelRoomType);
                    responseModel.Message = "Room Type  added successfully.";
                }

                await _context.SaveChangesAsync();
                responseModel.isSuccess = true;
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"Error occurred while adding or updating the subject: {ex.Message}";
            }

            return  responseModel;
        }

         
        public async Task<ResponseModel> DeleteRoomTypeById(Guid id)
        {
            var responseModel = new ResponseModel();

            try
            {
                // Find the subject by ID
                var hostelroomtype = await _context.HostelRoomTypes.FindAsync(id);
                if (hostelroomtype == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.Message = "hostel room type not found.";
                    return responseModel;
                }

                // Remove the subject
                _context.HostelRoomTypes.Remove(hostelroomtype);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "Room Type deleted successfully.";
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
