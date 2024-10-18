using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Parents;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.Students
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string? CampusId { get; set; }
        public string? UserId { get; set; }

        public string FullName { get; set; }


        public string? Surname { get; set; }
        public string? Cast { get; set; }
        public string? RelationWithParent { get; set; }
        public string? RelationWithGuardian { get; set; }



        public string Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }


        public string? PlaceOfBirth { get; set; }


        public string? ParentName { get; set; }


        public string? ParentCNIC { get; set; }



        public string? ParentEmail { get; set; }


        public string? ParentContactNo { get; set; }


        public string? ParentOtherNo { get; set; }


        public string? ParentOccupation { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? ParentIncome { get; set; }



        public DateTime? AdmissionDate { get; set; }

        public string? Religion { get; set; }
        public string? PreviousSchoolName { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? PreviousObtainedMarks { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? PreviousTotalMarks { get; set; }
        public string? PreviousClass { get; set; }
        public string? PreviousRemarks { get; set; }



        public string? Address { get; set; }

        public Guid? ClassId { get; set; }
        public Guid? ParentId { get; set; }
        public Class? Class { get; set; }
        public Guid? SectionId { get; set; }
        public Section? Section { get; set; }

        public string? Shift { get; set; }



        public string? GuardianName { get; set; }

        public string? GuardianContactNo { get; set; }
        public string? GuardianOtherNo { get; set; }


        public string? GuardianOccupation { get; set; }
		[Column(TypeName = "decimal(18, 2)")]
		public decimal? GuardianIncome { get; set; }

        public string? GuardianCNIC { get; set; }



        public Parent Parent { get; set; }  // Navigation property

        public string? GuardianEmail { get; set; }

        public string? StudentProfileUrl { get; set; }
        public string? GuardianProfileUrl { get; set; }
        public string? SchoolLeavingCertificateUrl { get; set; }
        public string? StudentFormBUrl { get; set; }
        public string? GuardianCNICFrontUrl { get; set; }
        public string? GuardianCNICBackUrl { get; set; }
        public string? OtherDocumentsUrl1 { get; set; }
        public string? OtherDocumentsTitle1 { get; set; }
        public string? OtherDocumentsUrl2 { get; set; }
        public string? OtherDocumentsTitle2 { get; set; }
    }
}
