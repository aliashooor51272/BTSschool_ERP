using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.HR;

namespace ByteTechSchoolERP.DataAccess.HRModels
{
    public class Salary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SalaryId { get; set; }

        public Guid EmployeeId { get; set; }
        public string RoleId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal DeductionAmount { get; set; }
        public string DeductionType { get; set; }
        public DateTime SalaryDate { get; set; }

        // Navigation property for User
        public virtual Employee Employee { get; set; }

    }
}
