using Microsoft.AspNetCore.Identity;

namespace Construction.Entity.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
    }
}
