using ByteTechSchoolERP.Models.Subjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.ClassAndSection
{
	public class SubjectAllocation
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid? TeacherId { get; set; }
		public Guid? ClassId { get; set; }
		public Guid? SubjectId { get; set; }
		public Guid? SectionId { get; set; }
		public string? UserId { get; set; }

		public DateTime? Createdon = DateTime.Today;
        // Navigation Properties
        public Class Class { get; set; } // Navigation property for Class
        public Section Section { get; set; } // Navigation property for Section
        public SubjectModel Subject { get; set; } // Navigation property for Subject

    }
}
