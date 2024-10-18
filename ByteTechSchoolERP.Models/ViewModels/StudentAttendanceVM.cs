using ByteTechSchoolERP.Models.ClassAndSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class StudentAttendanceVM
    {
        public Guid StudentIds { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        public Guid PromoteClassId { get; set; }
        public Class Class { get; set; }
        public DateTime AttendanceDate { get; set; }      // Date of Attendance

        public Guid PromoteSectionId { get; set; }
        public string Status { get; set; }      // Attendance status (e.g., Present, Absent, Late, On Leave)
        public string? Remarks { get; set; }
    }
}
