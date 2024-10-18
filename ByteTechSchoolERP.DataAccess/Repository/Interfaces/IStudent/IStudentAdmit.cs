using ByteTechSchoolERP.Models.ViewModels;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudent
{
    public interface IStudentAdmit
    {
        Task<ResponseModel> CreateStudentAdmit(StudentViewModel std);

    }
}
