using System.ComponentModel.DataAnnotations;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class CreateCampusViewModel
    {
        public string? CampusId { get; set; }
        public string? UserId { get; set; }
        [Required]
        [Display(Name = "Campus Name")]
        public string CampusName { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Area")]
        public string Area { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
