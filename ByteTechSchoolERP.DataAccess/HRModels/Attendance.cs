using ByteTechSchoolERP.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.DataAccess.HRModels
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid Id { get; set; }
        public string? Notes { get; set; }
        public bool? AttendanceStatus { get; set; }
        public DateTime? AttendancesDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdateBy { get; set; }
        public int EmployeeId { get; set; }

        public Guid StaffId { get; set; }
        public Staff? Staff { get; set; }
    }
}
