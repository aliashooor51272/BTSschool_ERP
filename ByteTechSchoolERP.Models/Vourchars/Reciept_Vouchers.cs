using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.Accounts;

namespace ByteTechSchoolERP.Models.Vourchars
{
     public class Receipt_Vouchers 
    {
        [Key]
        public int RV_ID { get; set; }  // Foreign key

        public int VoucherNum { get; set; }

        public DateTime VoucherDate { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Manual_Voucher_No { get; set; }
        public string? Drawn_Bank { get; set; } 
        public string Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public double Amount { get; set; }
        public string? Type { get; set; }
        public int? quantity { get; set; }
        public int? PreviousQuantity { get; set; }
        public string? ItemFlow { get; set; }

        public Nullable<bool> IsChecked { get; set; }  // True if checked
        public Nullable<bool> IsApproved { get; set; } // True if approved
        public int? InventoryId { get; set; }
        [ForeignKey("InventoryId")]
        public Inventory? Inventory { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }
        public string? Image6 { get; set; }
        public string? Image7 { get; set; }
        public string? Image8 { get; set; }
        public string? Image9 { get; set; }
        public string? Image10 { get; set; }
        public virtual ICollection<Reciept_Voucher_payment> Receipt_Voucher_Payments { get; set; } = new List<Reciept_Voucher_payment>();
    }
}
