using Hermes_Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hermes_Models
{
    public class Group : TEntity
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
        public virtual ICollection<UsersInGroup> Users { get; set; }

        public Group()
        {
            Users = new List<UsersInGroup>();
        }
    }
}
