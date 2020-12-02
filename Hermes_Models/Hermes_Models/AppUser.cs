using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Hermes_Models
{
    public class AppUser : IdentityUser
    {
        public virtual ICollection<UsersInGroup> UsersInGroup { get; set; }
        public bool IsConnected { get; set; }
    }

}
