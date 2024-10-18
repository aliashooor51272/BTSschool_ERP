using ByteTechSchoolERP.Models.ClassAndSection;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ByteTechSchoolERP.Models.ViewModels
{
    public class StudentViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string? Surname { get; set; }
        public string Cast { get; set; }
        public string RelationWithParent { get; set; }
        public string RelationWithGuardian { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string? PlaceOfBirth { get; set; }

        [StringLength(100)]
        public string? ParentName { get; set; }

        [StringLength(15)]
        public string? ParentCNIC { get; set; }

  
        [StringLength(100)]
        public string? ParentEmail { get; set; }

        [StringLength(15)]
        public string? ParentContactNo { get; set; }

        [StringLength(15)]
        public string? ParentOtherNo { get; set; }

        [StringLength(100)]
        public string? ParentOccupation { get; set; }

        public decimal? ParentIncome { get; set; }


        public DateTime? AdmitionDate { get; set; }
        [Required]
        public string? Religion { get; set; }
        public string? PreviousSchoolName { get; set; }
        public decimal? PreviousObtainedMarks { get; set; }
        public decimal? PreviousTotalMarks { get; set; }
        public string? PreviousClass { get; set; }
        public string? PreviousRemarks { get; set; }


        [MaxLength]
        public string? Address { get; set; }

        public Guid? ClassId { get; set; }
        public Class? Class { get; set; }
        public Guid? SectionId { get; set; }
        public Section? Section { get; set; }
        [Required]
        public string Shift { get; set; }


        [StringLength(100)]
        public string? GuardianName { get; set; }
        [StringLength(15)]
        public string? GuardianContactNo { get; set; }
        public string? GuardianOtherNo { get; set; }

        [StringLength(100)]
        public string? GuardianOccupation { get; set; }

        public decimal? GuardianIncome { get; set; }
        [StringLength(15)]
        public string GuardianCNIC { get; set; }

        [StringLength(100)]
        public string? GuardianEmail { get; set; }
        public DateTime? AdmissionDate { get; set; }
        [Required]
        public string StudentProfileUrl { get; set; }
        public IFormFile? StudentProfileUrlPathMV { get; set; }

        public string? GuardianProfileUrl { get; set; }
        public IFormFile? GuardianProfileUrlPathMV { get; set; }

        public string? SchoolLeavingCertificateUrl { get; set; }
        public IFormFile? SchoolLeavingCertificateUrlPathMV { get; set; }

        public string? StudentFormBUrl { get; set; }
        public IFormFile? StudentFormBUrlPathMV { get; set; }

        public string? GuardianCNICFrontUrl { get; set; }
        public IFormFile? GuardianCNICFrontUrlPathMV { get; set; }

        public string? GuardianCNICBackUrl { get; set; }
        public IFormFile? GuardianCNICBackUrlPathMV { get; set; }

        public string? OtherDocumentsUrl1 { get; set; }
        public IFormFile? OtherDocumentsUrl1PathMV { get; set; }

        public string? OtherDocumentsTitle1 { get; set; }
       

        public string? OtherDocumentsUrl2 { get; set; }
        public IFormFile? OtherDocumentsUrl2PathMV { get; set; }

        public string? OtherDocumentsTitle2 { get; set; }
 
        public IFormFile? DocumnetFileVM { get; set; }

    }
}
