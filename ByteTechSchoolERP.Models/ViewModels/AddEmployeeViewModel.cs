using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; } // Adjust type if necessary
        public string Name { get; set; }
    }

    public class AddEmployeeViewModel
    {
        public List<RoleViewModel> Roles { get; set; }
        public List<string> Departments { get; set; }

        // Additional properties can be added here if needed
        public string SelectedRoleId { get; set; } // For selected role in the form
        public string SelectedDepartment { get; set; } // For selected department in the form
    }

}
