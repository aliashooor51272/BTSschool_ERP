using System.ComponentModel.DataAnnotations;

namespace ByteTechSchoolERP.Models
{
    public class ResponseModel
    {
        public bool isSuccess { get; set; }

        [MaxLength]
        public string? Message { get; set; }
     }
}
