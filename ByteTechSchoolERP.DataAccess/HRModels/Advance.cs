using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.DataAccess.HRModels
{
    public class Advance
    {
        public int AdvanceId { get; set; }
        public DateTime AdvanceDate { get; set; }
		[Column(TypeName = "decimal(18, 2)")]  
		public decimal AdvanceAmount { get; set; }
        public Guid StaffId { get; set; }

        public Staff? Staff { get; set; }
        // Other advance details like repayment terms, status, etc.
    }
}
