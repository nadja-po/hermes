using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes_Models
{
    public class UsersInGroup 
    {
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

        
    }

}

