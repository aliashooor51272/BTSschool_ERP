using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class ClassSectionSubjectViewModel
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public Guid? SectionId { get; set; }
        public string SectionName { get; set; }
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }

        // Add these properties to match the diary data
        public DateTime HomeworkDate { get; set; }
        public DateTime SubmissioDate { get; set; }
        public string? AttachDocument { get; set; }
        public string Description { get; set; }
        public string? DiaryDescription { get; set; } // Renamed to avoid conflict with `Description` from ClassSection


        public string StudentId { get; set; }
        public string FullName { get; set; }
        public string RollNo { get; set; } // Assuming you have a RollNo or similar identifier
                                           // Add other properties as needed
    }
}
