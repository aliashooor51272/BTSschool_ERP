using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Accounts
{
    public class Element_Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Element_Account_Code { get; set; }
        public string? Account_Title { get; set; }
        public ICollection<Control_Account>? Control_Accounts { get; set; }
        public ICollection<Ledger_Account>? Ledger_Accounts { get; set; }

    }
}
