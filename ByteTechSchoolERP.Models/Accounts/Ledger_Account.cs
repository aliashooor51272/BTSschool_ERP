using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteTechSchoolERP.Models.Accounts
{
    public class Ledger_Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Ledger_Account_Code { get; set; }
        [Required]
        public string? Ledger_Account_Title { get; set; }
        public string? Ledger_Complete_Code { get; set; }
        public double? Balance { get; set; }
        //form the party table
        public int Element_AccountId { get; set; }
        [ForeignKey("Element_AccountId")]
        [Required]
        public Element_Account? Element_Account { get; set; }
        public int Control_AccountId { get; set; }
        [ForeignKey("Control_AccountId")]
        [Required]
        public Control_Account? Control_Account { get; set; }

        public ICollection<Inventory>? Inventories { get; set; }

    }
}
