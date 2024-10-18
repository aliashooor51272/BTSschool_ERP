using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.Exam
{
   
    public class Term
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
