using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Exam
{
	public class ExamType
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string? ExamTypeName { get; set; }
		public string? Description { get; set; }
		 

	}
}
