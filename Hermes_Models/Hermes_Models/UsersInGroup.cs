﻿using Hermes_Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hermes_Models
{
    public class UsersInGroup : TEntity
    {
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }

    }

}

