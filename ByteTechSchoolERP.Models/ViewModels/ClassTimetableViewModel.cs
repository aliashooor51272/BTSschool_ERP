using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class ClassTimetableViewModel
    {
        public string Day { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }
        public List<TimetableEntry> TimetableEntries { get; set; }
    }


    public class TimetableEntry
    {
        public Guid SubjectId { get; set; }
        public Guid StaffTempId { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string RoomNo { get; set; }
    }
}
