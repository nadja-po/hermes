using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Hermes_Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Messages = new HashSet<Message>();
        }
       

        public virtual ICollection<Message> Messages { get; set; }
    }
}
