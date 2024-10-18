using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudent;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Students;
using ByteTechSchoolERP.Models.ViewModels;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.StudentService
{
    public class StudentAttendanceRepo : GenericRepository<StudentAttendanceVM>, IStudentAttendance
    {
        private readonly ByteTechSchoolERPContext _context;

        public StudentAttendanceRepo(ByteTechSchoolERPContext db) : base(db)
        {
            _context = db;
        }


        public async Task<ResponseModel> MarkStudentAttendance(List<StudentAttendanceVM> studentAttendances)
        {
            var responseModel = new ResponseModel();

            if (studentAttendances == null || studentAttendances.Count == 0)
            {
                responseModel.isSuccess = false;
                responseModel.Message = "Attendance data is required.";
                return responseModel;
            }

            try
            {
                foreach (var data in studentAttendances)
                {
                    var attendance = new StudentAttendance
                    {
                        StudentId = data.StudentIds,
                        AttendanceDate = data.AttendanceDate, // Assuming AttendanceDate exists in StudentAttendanceVM
                        Status = data.Status,
                        Remarks = data.Remarks,
                        PromoteClassId = data.PromoteClassId,
                        PromoteSectionId = data.PromoteSectionId
                    };

                    _context.StudentAttendances.Add(attendance);
                }

                await _context.SaveChangesAsync();
                responseModel.isSuccess = true;
                responseModel.Message = "Attendance submitted successfully!";
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"An error occurred while submitting attendance: {ex.Message}";
            }

            return responseModel;
        }

    }
}

