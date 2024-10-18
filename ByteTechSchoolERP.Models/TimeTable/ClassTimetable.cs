using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.TimeTable
{
    public class ClassTimetable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid StaffTempId { get; set; }
        public string Day { get; set; }
        public TimeOnly? Startfrom { get; set; }
        public TimeOnly? Startto { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ICollection<TimetableEntries>? timetableEntries { get; set; }
    }

}
