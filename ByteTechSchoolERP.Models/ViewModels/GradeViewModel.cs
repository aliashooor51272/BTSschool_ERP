using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class GradeViewModel
    {
        public Guid Id { get; set; }
        public string? GradeTitle { get; set; }
        public string? Description { get; set; }
        public string? Grades { get; set; }
        public string? MaximumPercentage { get; set; }
        public string? MinimumPercentage { get; set; }
        public string? Remark { get; set; }
    }
}
