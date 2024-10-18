using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Exam
{
	public class ExamList
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string? ExamName { get; set; }
		public string? Description { get; set; }
		public Guid? TermId { get; set; }
		public Guid? ClassId { get; set; }
		public Guid? SeactionId { get; set; }
		public Guid? GradeId { get; set; }
		public string? Session { get; set; }
		public string? AddExamDate { get; set; }
		public bool? IsPublished { get; set; }
	 

	}
}
