using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class TermViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Add this property if it was missing
        public string? Description { get; set; }

    }
}
