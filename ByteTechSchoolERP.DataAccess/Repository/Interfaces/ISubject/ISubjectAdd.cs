using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISubject
{
    public interface ISubjectAdd
    {
        Task<ResponseModel> AddOrUpdateSubject(SubjectModel subjectModel);
        List<SubjectViewModel> GetSubjectLists(SubjectModel subjectModel);
        Task<ResponseModel> DeleteSubjectById(Guid id);
    }
}
