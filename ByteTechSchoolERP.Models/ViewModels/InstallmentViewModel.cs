using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class InstallmentViewModel
    {
        public Guid UserId { get; set; }
        public decimal PerInstallmentAmount { get; set; }
        public DateTime DueDate { get; set; }
    }
}
