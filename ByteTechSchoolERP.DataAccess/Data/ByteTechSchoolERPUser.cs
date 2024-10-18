 using Microsoft.AspNetCore.Identity;

namespace ByteTechSchoolERP.DataAccess.Data
{
    public class ByteTechSchoolERPUser : IdentityUser
    {
        public Guid CampusId { get; set; }
        public Guid StaffTempId { get; set; }
        public string? CNICNumber { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
