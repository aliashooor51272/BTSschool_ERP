using ByteTechSchoolERP.DataAccess.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.DataAccess.HRModels
{
    public class Payroll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PayrollId { get; set; }

        public DateTime PayrollDate { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
