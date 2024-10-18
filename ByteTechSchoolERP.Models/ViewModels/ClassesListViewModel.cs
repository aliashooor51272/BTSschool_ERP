using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class ClassesViewModel
    {
        public Guid Id { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        public string ClassName { get; set; }
        public string? Description { get; set; }
    }
}
