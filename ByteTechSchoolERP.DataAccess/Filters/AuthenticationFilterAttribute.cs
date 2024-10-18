using Microsoft.AspNetCore.Mvc;

namespace ByteTechSchoolERP.DataAccess.Filters
{
    public class AuthenticationFilterAttribute : TypeFilterAttribute
    {
        public AuthenticationFilterAttribute() : base(typeof(AuthenticationFilter))
        {
        }
    }
}
