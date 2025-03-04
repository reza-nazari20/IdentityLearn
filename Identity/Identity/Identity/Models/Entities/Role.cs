using Microsoft.AspNetCore.Identity;

namespace Identity.Models.Entities
{
    public class Role:IdentityRole
    {
        public string Description { get; set; }
    }
}
