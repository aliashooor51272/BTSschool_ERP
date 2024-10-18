using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace ByteTechSchoolERP.Models.ViewModels
{
	public class LoanViewModel
	{
		public string UserId { get; set; }
		public List<SelectListItem> Users { get; set; } // For user dropdown
		public decimal TotalAmount { get; set; }
		public int TotalInstallments { get; set; }
		public decimal InstallmentAmount { get; set; } // Readonly field
	}

}
