using ByteTechSchoolERP.Models.Lesson;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using ByteTechSchoolERP.Models.ViewModels;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClasstimetable
{
    public interface IClasstimetable
    {
        Task<ResponseModel> SaveTimetableAsync(ClassTimetableViewModel model);
    }
}
