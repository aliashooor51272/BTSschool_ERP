using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.ClassAndSection;

namespace ByteTechSchoolERP.Models.Students
{
    public class StudentAttendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StudentAttendanceId { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        public Student? Student { get; set; }
        public Guid? StudentId { get; set; }      // Foreign Key to Student
        public DateTime AttendanceDate { get; set; }      // Date of Attendance
        public string? Status { get; set; }      // Attendance status (e.g., Present, Absent, Late, On Leave)
        public string? Remarks { get; set; } // Attendance status (e.g., Present, Absent, Late, On Leave)

        public Class? Class { get; set; }
        public Guid? PromoteClassId { get; set; }
        public Section? Section { get; set; }
        public Guid? PromoteSectionId { get; set; }
    }
}
