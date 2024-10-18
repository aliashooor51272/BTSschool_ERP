using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteTechSchoolERP.Models.Accounts

{
    public class Control_Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Control_Account_Code { get; set; }
        [Required]
        public string? Control_Account_Title { get; set; }
        public string? Control_Complete_Code { get; set; }
        public int Element_AccountId { get; set; }
        [ForeignKey("Element_AccountId")]
        public Element_Account? Element_Account { get; set; }
        public ICollection<Ledger_Account>? Ledger_Accounts { get; set; }

    }
}
