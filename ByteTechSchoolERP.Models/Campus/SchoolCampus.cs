using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.Campus
{
    public class SchoolCampus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CampusId { get; set; }
        public string CampusName { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
    }
}
