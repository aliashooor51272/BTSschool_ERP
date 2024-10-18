using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class DiaryDataViewModel
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public DateTime HomeworkDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string AttachDocument { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }
}

