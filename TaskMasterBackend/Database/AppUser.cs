using Microsoft.AspNetCore.Identity;

namespace TaskMasterBackend.Database
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
