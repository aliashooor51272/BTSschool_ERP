using ByteTechSchoolERP.Models.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class AttendanceViewModel
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

}
