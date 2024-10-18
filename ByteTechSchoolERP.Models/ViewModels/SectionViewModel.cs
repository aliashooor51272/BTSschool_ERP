using ByteTechSchoolERP.Models.ClassAndSection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class SectionViewModel
    {
        public Guid Id { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }

        public Guid ClassId { get; set; }
        public Class Class { get; set; }
    }
}
