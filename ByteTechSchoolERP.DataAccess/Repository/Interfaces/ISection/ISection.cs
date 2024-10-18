using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.ViewModels;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISection
{
    public interface ISection
    {
        Task<ResponseModel> AddOrUpdateSection(Section section);
        List<SectionViewModel> GetSectionList();
        Task<ResponseModel> DeleteSectionById(Guid id);
    }
}
