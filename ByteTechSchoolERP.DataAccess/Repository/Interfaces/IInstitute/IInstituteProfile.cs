using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.InstitudesProfile;
using ByteTechSchoolERP.Models.ViewModels;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IInstituite
{
    public interface IInstituteProfile
    {
        Task<ResponseModel> AddIntitute(Instituite_VM instituite );
    }
}
