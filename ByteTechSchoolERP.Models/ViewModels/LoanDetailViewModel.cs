using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class LoanDetailViewModel
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } // Display full name in dropdown
        public decimal LoanAmount { get; set; }
        public int Installments { get; set; }
        public decimal PerInstallmentAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal RemainingAmount { get; set; }
        public int PaidInstallments { get; set; }
        public int RemainingInstallments { get; set; }
    }
}
