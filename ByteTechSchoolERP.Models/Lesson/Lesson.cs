using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.Lesson
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }
        public Guid SubjectId { get; set; }
        public string LessonName { get; set; }
    }
}
