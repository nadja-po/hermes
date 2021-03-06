﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hermes_Models
{
    public class Group 
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Group name")]
        public string GroupName { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public string ModeratorId { get; set; }

        public Group()
        {
            Users = new HashSet<IdentityUser>();
        }

        public virtual ICollection<IdentityUser> Users { get; set; }

        public Group ToData()
        {
            return new Group()
            {
                Id = this.Id,
                GroupName = this.GroupName,
                CreatorId = this.CreatorId,
                ModeratorId = this.ModeratorId,
            };
        }
        public static Group FromData(Group data)
        {
            return new Group()
            {
                Id = data.Id,
                GroupName = data.GroupName,
                CreatorId = data.CreatorId,
                ModeratorId = data.ModeratorId,
            };
        }
    }
}
