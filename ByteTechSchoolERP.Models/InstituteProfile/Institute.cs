using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteTechSchoolERP.Models.InstitudesProfile
{
    public class Institute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? CampusId { get; set; }
        public string? UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string? WebsiteUrl { get; set; }
        [Required]

        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        [Required]
        public string Country { get; set; }

        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        [Required]
        public string City { get; set; }

        public string? ImagePath { get; set; }
    }
}
