using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.Students;

namespace ByteTechSchoolERP.Models.HomeDiary
{
    public class HomeWork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? DiaryId { get; set; }
        public Guid StudentId { get; set; }
        public Guid SectionId { get; set; }
        public Guid ClassId { get; set; }
        public string SubjectName { get; set; }
        public DateTime SubmissioDate { get; set; }
        public DateTime Createdon { get; set;  }
        public string fileurl { get; set; }
        public Class Class { get; set; } // Navigation property for Class
        public Section Section { get; set; } // Navigation property for Subject
        public Student Student { get; set; } // Navigation property for Subject



    }
}
