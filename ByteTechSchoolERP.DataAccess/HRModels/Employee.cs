using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.DataAccess.HRModels;

namespace ByteTechSchoolERP.Models.HR
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EmployeeId { get; set; }
         public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? EmergencyContact { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Address { get; set; }
        public string? PermanentAddress { get; set; }
        public string? PANNumber { get; set; }
        public string? Qualification { get; set; }
        public string? WorkExperience { get; set; }
        public bool Status { get; set; }
        public decimal? Salary { get; set; } // Add this property

        // Foreign keys for relationships
        public string? DepartmentName { get; set; }
      
        public string? RoleId { get; set; }
        public IdentityRole Role { get; set; }
    }

}
