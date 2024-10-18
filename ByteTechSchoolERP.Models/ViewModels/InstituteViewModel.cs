using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class Instituite_VM
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
        
        public IFormFile? ImageFileVM { get; set; }
        public string? ImagePath { get; set; }
    }
}

