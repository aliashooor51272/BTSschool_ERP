using ByteTechSchoolERP.Models.ClassAndSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class PromoteStudentViewModel
    {
        public List<Guid> StudentIds { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        public Guid PromoteClassId { get; set; }
        public Class Class { get; set; }

        public Guid PromoteSectionId { get; set; }
    }
}
