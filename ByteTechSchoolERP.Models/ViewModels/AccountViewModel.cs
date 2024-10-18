using ByteTechSchoolERP.Models.Vourchars;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class AccountViewModel
    { 
      public Receipt_Vouchers? Receipt_Vouchers { get; set; }
    public Reciept_Voucher_payment? Reciept_Voucher_Payment  { get; set; }
    public List<Reciept_Voucher_payment>? Reciept_Voucher_Payments { get; set; }
        public FixData? FixData { get; set; }

        public string? ImagePath1 { get; set; }
    public string? ImagePath2 { get; set; }
    public string? ImagePath3 { get; set; }
    public string? ImagePath4 { get; set; }
    public string? ImagePath5 { get; set; }
    public string? ImagePath6 { get; set; }
    public string? ImagePath7 { get; set; }
    public string? ImagePath8 { get; set; }
    public string? ImagePath9 { get; set; }
    public string? ImagePath10 { get; set; }
    public IFormFile? image1 { get; set; }
    public IFormFile? image2 { get; set; }
    public IFormFile? image3 { get; set; }
    public IFormFile? image4 { get; set; }
    public IFormFile? image5 { get; set; }
    public IFormFile? image6 { get; set; }
    public IFormFile? image7 { get; set; }
    public IFormFile? image8 { get; set; }
    public IFormFile? image9 { get; set; }
    public IFormFile? image10 { get; set; }


}
}
