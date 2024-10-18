using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.Subjects
{
    public class SubjectModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }

        [Required]
        public string? SubjectName { get; set; }
        [Required]
        public string? SubjectCode { get; set; }
        public int? SubjectTotalMarks { get; set; }
    }
}
