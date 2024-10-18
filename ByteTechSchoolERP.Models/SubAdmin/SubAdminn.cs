using ByteTechSchoolERP.Models.Campus;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.SubAdmin
{
    public class SubAdminn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
      
        public string CNICNumber { get; set; }
        public string CampusName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool Status { get; set; }
        public IEnumerable<SchoolCampus> Campuses { get; set; }
    }
}
