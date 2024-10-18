using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class ExamTemplateViewModel
    {
        [Required]
        public string TemplateName { get; set; }

        [Required]
        public string ExamName { get; set; }

        [Required]
        public string SchoolName { get; set; }

        [Required]
        public string ExamCenter { get; set; }

        public string BodyText { get; set; }

        public string FooterText { get; set; }

        [Required]
        public DateTime PrintingDate { get; set; }

        public IFormFile HeaderImage { get; set; }

        public IFormFile LeftLogo { get; set; }

        public IFormFile RightLogo { get; set; }

        public IFormFile LeftSign { get; set; }

        public IFormFile MiddleSign { get; set; }

        public IFormFile RightSign { get; set; }

        public IFormFile BackgroundImage { get; set; }
    }
}
