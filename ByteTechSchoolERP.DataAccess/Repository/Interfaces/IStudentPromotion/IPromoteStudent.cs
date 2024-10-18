using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.Students;
using ByteTechSchoolERP.Models.ViewModels;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudentPromotion
{
    public interface IPromoteStudent
    {
        Task<ResponseModel> CreateStudentPromotion(PromoteStudentViewModel studentModel);

    }
}
