using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class AddRoleViewModel
    {
        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
    }
}
