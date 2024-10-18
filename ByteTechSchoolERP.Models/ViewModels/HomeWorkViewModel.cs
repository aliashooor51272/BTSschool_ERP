using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class HomeWorkViewModel
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public DateTime SubmissioDate { get; set; }
        public DateTime Createdon { get; set; }
        public string FileUrl { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
    }

}
