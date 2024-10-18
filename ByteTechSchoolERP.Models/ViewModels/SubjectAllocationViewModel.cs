using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class SubjectAllocationViewModel
    {
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public int? SubjectMarks { get; set; }
        public Guid TeacherId { get; set; }
    }
}
