
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace ByteTechSchoolERP.Models.ViewModels
{
	public class LoanApplicationViewModel
	{
        public Guid UserId { get; set; } // Selected User ID
        public decimal LoanAmount { get; set; } // Loan Amount
        public int NumberOfInstallments { get; set; } // Number of Installments
        public decimal PerInstallmentAmount { get; set; } // Calculated Per Installment Amount
        public DateTime DueDate { get; set; } // Due Date
        public DateTime PaidDate { get; set; } // Paid Date
        public decimal RemainingAmount { get; set; } // Remaining Amount
        public int PaidInstallments { get; set; } // Number of Paid Installments
        public int RemainingInstallments { get; set; } // Remaining Installments
         public IEnumerable<SelectListItem> Users { get; set; } // Add this property

    }
}
