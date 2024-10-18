using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Accounts
{
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Item_Name { get; set; }       
        public int OpeningQuantity { get; set; }
        public string? UnitOfMeasure { get; set; }
        public int? Ledger_AccountId { get; set; }
        [ForeignKey("Ledger_AccountId")]
        public Ledger_Account? Ledger_Account { get; set; }
    }
}
