using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Exam
{
	public class Grade
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string? GradeTitle { get; set; }
		public string? Description { get; set; }
		public string? Grades { get; set; }
		public string? MaximumPercentage { get; set; }
		public string? MinimumPercentage { get; set; }
		public string? Remark { get; set; }
		 

	}
}
