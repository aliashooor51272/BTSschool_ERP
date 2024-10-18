using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ByteTechSchoolERP.Models.Campus;

namespace ByteTechSchoolERP.Models
{
    public class CreateSubAdminViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile Attachment { get; set; }

        public string CNICNumber { get; set; }
        public string AttachmentUrl { get; set; }
        public Guid CampusId { get; set; }
        public string CampusName { get; set; }

        // New fields for password
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IEnumerable<SchoolCampus> Campuses { get; set; }
    }
}
