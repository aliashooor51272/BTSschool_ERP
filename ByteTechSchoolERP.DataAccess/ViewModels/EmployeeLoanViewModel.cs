using ByteTechSchoolERP.DataAccess.HRModels;
using ByteTechSchoolERP.Models.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.ViewModels
{
    public class EmployeeLoanViewModel
    {
        public Employee Employee { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
