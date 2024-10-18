using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Exam;
using ByteTechSchoolERP.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass
{
    public interface ITerm
    {
        Task<ResponseModel> AddOrUpdateTerm(Term term);
        List<TermViewModel> GetTermList(); // Return type should be List<TermViewModel>
        Task<ResponseModel> DeleteTermById(Guid id);
    }
}
