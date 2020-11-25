using System.Collections.Generic;
using System.Linq;
using Hermes_Services.Data;
using Hermes_Models;
using Microsoft.AspNetCore.Identity;

namespace Hermes_Services.Repositories
{

    public class GroupRepository
    {
        private readonly ApplicationDbContext db;

        public GroupRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Group Get(int id)
        {
            return this.db.Group.FirstOrDefault(n => n.Id == id);
        }

       
        public List<Group> GetAllGroups()
        {
            return this.db.Group.ToList();
        }
        public List<UsersInGroup> GetUsersByGroup(int groupId)
        {
            return this.db.UsersInGroup.Where(n => n.GroupId == groupId).ToList();

        }
        
        public List<UsersInGroup> GetGroupByUser(string userId)
        {
            return this.db.UsersInGroup.Where(n => n.UserId == userId).ToList();
        }


        public int GetNumberUsersInGroup(int groupId)
        {
            return this.db.UsersInGroup.Count(n => n.GroupId == groupId);
        }
        public Group GetByName(string groupName)
        {
            return this.db.Group.FirstOrDefault(n => n.GroupName == groupName);
        }

        public IdentityUser GetUserById(string id)
        {
            return this.db.Users.FirstOrDefault(n => n.Id == id);
        }

        public int CreateGroup(Group group)
        {
            this.db.Group.Add(group);
            this.db.SaveChanges();
            var id = group.Id;
            return id;
        }

        public void DeleteGroup(int id)
        {
            var group = this.db.Group.FirstOrDefault(t => t.Id == id);
            this.db.Group.Remove(group);

            this.db.SaveChanges();
        }
        public void AddUserInGroup(int groupId, string userId)
        {
            UsersInGroup userInGroup = new Hermes_Models.UsersInGroup()
            {
                GroupId = groupId,
                UserId = userId,
            };
            this.db.UsersInGroup.Add(userInGroup);
            this.db.SaveChanges();
        }

        public void DeleteUserIntoGroup(int groupId, string userId)
        {
            var user = this.db.UsersInGroup.Where(t => t.GroupId == groupId).FirstOrDefault(t => t.UserId == userId);
            this.db.UsersInGroup.Remove(user);
            this.db.SaveChanges();
        }
        public UsersInGroup GetUserInGroup(int groupId, string userId)
        {
            var user = this.db.UsersInGroup.Where(t => t.GroupId == groupId).FirstOrDefault(t => t.UserId == userId);
            return user;
        }
        public void ChangeModeratorInGroup(int groupId, string nextModeratorId)
        {
            var group = this.db.Group.FirstOrDefault(t => t.Id == groupId);
            group.ModeratorId = nextModeratorId;
            this.db.SaveChanges();
        }

        public Group ChangeModerator(int id)
        {
            return this.db.Group.FirstOrDefault(n => n.Id == id);
        }

        public UsersInGroup GetUserInGroupBiId(int id)
        {
            return this.db.UsersInGroup.FirstOrDefault(n => n.Id == id);

        }

    }

}
    