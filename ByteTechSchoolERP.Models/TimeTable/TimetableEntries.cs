using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.TimeTable
{
   
       public class TimetableEntries
       {
        [Key]
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Guid StaffTempId { get; set; }
        public string? TimeFrom { get; set; }
        public string? TimeTo { get; set; }
        public string? RoomNo { get; set; }
        public Guid ClassTimetableId { get; set; }
        [ForeignKey(nameof(ClassTimetableId))]

        public virtual ClassTimetable? ClassTimetable { get; set; }
    }

}


