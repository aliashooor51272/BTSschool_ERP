using ByteTechSchoolERP.DataAccess.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.DataAccess.HRModels
{
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
      
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? FatherName { get; set; }
        [Required]
        public string Gender { get; set; } 

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string? ContactNumber { get; set; }
        public string? Cnic { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; }
        [Phone]
        public string? EmergencyNumber { get; set; }
        public string? MaritalStatus { get; set; }  
        public string? TemporaryAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? Note { get; set; }
        public string? AccountTitle { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountHolder { get; set; }
        public string? AccountBankBranchNameHolder { get; set; }
        public decimal? BasicSalary { get; set; }
        public string? ContractType { get; set; }
        public string? Qualification { get; set; }
        public int? NumberOfExperience { get; set; }
        public string? ResumeUrl { get; set; }  
        public string? ImageUrl { get; set; }  
        public string? DegreeUrl { get; set; }  
        public DateTime? CreatedDate { get; set; }  
        public DateTime? UpdatedDate { get; set; }  
        public string? CreatedBy { get; set; }  
        public string? UpdateBy { get; set; }
        public string? StaffNumber { get; set; }
        public long? StaffRegistionNumber { get; set; }

        public string? RoleId { get; set; }
        public string? UserId { get; set; }
        public Guid? DeparmentId { get; set; }
        public string? Designation { get; set; }
        public ByteTechSchoolERPUser User { get; set; }
        public required virtual Department Department { get; set; }
        public virtual ICollection<Payroll>? Payrolls { get; set; }
        public virtual ICollection<Advance>? Advances { get; set; }
        public virtual ICollection<Loan>? Loans { get; set; }
        public virtual ICollection<Attendance>? Attendances { get; set; }


    }
}
