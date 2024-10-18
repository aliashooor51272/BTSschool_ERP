using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class MarksViewModel
    {
        public string Userid { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public double TotalMarks { get; set; }
        public double ObtainedMarks { get; set; }
        public double Percentage { get; set; }
        public string Grade { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public string StudentName { get; set; }
  
        public DateTime CreatedOn { get; set; }
    }
}
