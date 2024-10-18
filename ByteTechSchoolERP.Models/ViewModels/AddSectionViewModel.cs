using ByteTechSchoolERP.Models.ClassAndSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class AddSectionViewModel
    {
        
            public List<Class> Classes { get; set; }
            public List<Section> Sections { get; set; }

        public string? CampusId { get; set; }
        public string? UserId { get; set; }
    }

  
}