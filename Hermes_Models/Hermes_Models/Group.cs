using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hermes_Models
{
    public class Group 
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Group name")]
        public string GroupName { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public string ModeratorId { get; set; }

        public Group()
        {
            Users = new HashSet<UsersInGroup>();
        }

        public virtual ICollection<UsersInGroup> Users { get; set; }

        public Group ToData()
        {
            return new Group()
            {
                Id = this.Id,
                GroupName = this.GroupName,
                CreatorId = this.CreatorId,
            };
        }
        public static Group FromData(Group data)
        {
            return new Group()
            {
                Id = data.Id,
                GroupName = data.GroupName,
                CreatorId = data.CreatorId,
            };
        }
    }
}
