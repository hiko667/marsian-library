using Microsoft.AspNetCore.Identity;

namespace marsian_library.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? ReaderId {get; set;}
        public virtual Reader? Reader {get; set;}
        public int? EmpId {get; set;}
        public virtual Emp? Emp {get; set;}
    }
}