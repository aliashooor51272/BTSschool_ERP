using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Subjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.HomeDiary
{
    public class Diary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectionId
        {
            get; set;
        }// Corrected from SectonId to SectionId
        public Guid SubjectId { get; set; }
        public DateTime HomeworkDate { get; set; }
        public DateTime SubmissioDate { get; set; }
        public string? AttachDocument { get; set; }
        public string Description { get; set; }
        public string? UserId { get; set; }
        public Class Class { get; set; } // Navigation property for Class
          public SubjectModel Subject { get; set; } // Navigation property for Subject
          public Section Section { get; set; } // Navigation property for Subject
    }
}

