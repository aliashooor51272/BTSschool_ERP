using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Exam
{
	public class ExamTemplate
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string? TemplateName { get; set; }
		public string? UserId { get; set; }
		public string? ExamName { get; set; }
		public string? SchoolName { get; set; }
		public string? ExamCenter { get; set; }
		public string? BodyText { get; set; }
		public string? FooterText { get; set; }
		public DateTime? PrintingDate { get; set; }

		// File URLs
		public string? HeaderImageUrl { get; set; }
		public string? LeftLogoUrl { get; set; }
		public string? RightLogoUrl { get; set; }
		public string? LeftSignUrl { get; set; }
		public string? MiddleSignUrl { get; set; }
		public string? RightSignUrl { get; set; }
		public string? BackgroundImageUrl { get; set; }
		public DateTime? createdon = DateTime.Today;

	}
}
