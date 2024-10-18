using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.Models.HR;

namespace ByteTechSchoolERP.DataAccess.HRModels
{
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public decimal LoanAmount { get; set; }
        public int NumberOfInstallments { get; set; }
        public decimal PerInstallmentAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal RemainingAmount { get; set; }
        public int PaidInstallments { get; set; }
        public int RemainingInstallments { get; set; }


        // Navigation property for User
        public virtual Employee Employee { get; set; }
    }

}
