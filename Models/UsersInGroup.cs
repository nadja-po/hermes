using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes_chat.Models
{
    public class UsersInGroup : AppUser
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
