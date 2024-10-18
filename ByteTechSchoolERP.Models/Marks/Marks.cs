using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.Students;
using ByteTechSchoolERP.Models.Exam;

namespace ByteTechSchoolERP.Models.Marks
{
    public class Marks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Userid { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } // Navigation property

        public Guid? StudentId { get; set; }
        public Guid? Subject { get; set; }
        public Double? TotalMarks { get; set; }
        public Double? ObtainMarks { get; set; }
        public String? Grade { get; set; }
        public double? Percentage { get; set; }
        public string? ExamId { get; set; }
        public ExamList ExamList { get; set; }
        public ExamSchedular ExamSchedular { get; set; }

        public DateTime CreatedOn = DateTime.Now;
    }
}
