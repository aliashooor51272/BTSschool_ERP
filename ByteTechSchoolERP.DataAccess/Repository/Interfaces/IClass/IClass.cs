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

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass
{
    public interface IClass
    {
        Task<ResponseModel> AddOrUpdateClasses(Class classes);
        List<ClassesViewModel> GetClassList();
        Task<ResponseModel> DeleteClassById(Guid id);
       
    }
}
