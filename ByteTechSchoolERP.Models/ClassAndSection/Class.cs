using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByteTechSchoolERP.Models.ClassAndSection
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string? CampusId { get; set; }
        public string? UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string ClassName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public ICollection<Section>? Sections { get; set; }

    }
}
