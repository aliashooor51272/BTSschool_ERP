using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class ExamViewModel
    {
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
