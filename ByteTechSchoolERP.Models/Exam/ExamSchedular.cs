using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Exam
{
	public class ExamSchedular
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		 
		public Guid? ExamId { get; set; }
		public Guid? SubjectId { get; set; }
		public DateTime? Date { get; set; }
		public TimeOnly? StartTime { get; set; }
		public int? Duration { get; set; }
		public string? Roomno { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? Createdby { get; set; }
		public DateTime? UpdateDate { get; set; }
		public string? Updateby { get; set; }
		 
	 

	}
}
