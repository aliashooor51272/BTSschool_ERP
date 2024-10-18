using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CampusName { get; set; }
    }
}
