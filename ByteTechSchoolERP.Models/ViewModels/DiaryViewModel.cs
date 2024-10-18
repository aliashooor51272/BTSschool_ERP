using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class DiaryViewModel
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectonId { get; set; }
        public DateTime HomeworkDate { get; set; }
        public DateTime SubmissioDate { get; set; }
        public string Description { get; set; }
        public string AttachDocument { get; set; }
        public string SectionName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public Guid SubjectId { get; set; }
        public Guid DiaryId { get; set; } // Add this line
    }

}
