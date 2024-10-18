using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class TeacherSubjectAllocationViewModel
    {
        public Guid ClassId { get; set; }
        public string? ClassName { get; set; }
        public Guid SectionId { get; set; }
        public string? SectionName { get; set; }
        public Guid SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public int? SubjectTotalMarks { get; set; }
        public Guid ExamId { get; set; }  // New property
        public string ExamName { get; set; }  // New property
                                              // Add a property to hold the list of exams for the dropdown
      
    }
}
