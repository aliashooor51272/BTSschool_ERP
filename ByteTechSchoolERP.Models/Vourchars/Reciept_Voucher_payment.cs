using ByteTechSchoolERP.Models.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.Vourchars
{
    public class Reciept_Voucher_payment
    {
        [Key]
         public int RVP_ID { get; set; }

        public int? RV_ID { get; set; }  // Foreign key

        public int? Element_Account_Code { get; set; }
        public int? Control_Account_Code { get; set; }
        public int? Ledger_Account_Code { get; set; }
        public string Comments { get; set; }
        public double? Amount { get; set; }
        public double? PreviousBalance { get; set; }
        public double? CurrentBalance { get; set; }
        public double? RemainingBalance { get; set; }

        public string? TransType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Ledger_AccountId { get; set; }
        [ForeignKey("Ledger_AccountId")]
        public virtual Ledger_Account? Ledger_Account { get; set; }
        [ForeignKey("RV_ID")]
        public virtual Receipt_Vouchers? Receipt_Vouchers  { get; set; }

    }



}
