using ByteTechSchoolERP.DataAccess.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.DataAccess.HRModels
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public Guid StaffId { get; set; }

        public required List<Staff> Staffs { get; set; }
    }
}
