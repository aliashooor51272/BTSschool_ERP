using ByteTechSchoolERP.Models.ViewModels;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudent
{
    public interface IStudentAttendance
    {
        Task<ResponseModel> MarkStudentAttendance(List<StudentAttendanceVM> studentAttendances);

    }
}
