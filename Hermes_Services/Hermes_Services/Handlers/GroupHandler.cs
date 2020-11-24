using System.Linq;
using Hermes_Services.Repositories;
using Hermes_Models;
using System.Collections.Generic;
using Hermes_Services.Data;

namespace Hermes_Services.Handler
{

    public class GroupHandler
    {
        private GroupRepository _repository;

        public GroupHandler(ApplicationDbContext db)
        {
            _repository = new GroupRepository(db);
        }

        public Group GetById(int Id)
        {
            return _repository.GetById(Id);

        }
        //public List<UsersInGroup> GetGroupByUser(string userId)
        //{
        //    return db.UsersInGroup.Where(n => n.UserId == userId).ToList();
        //}

        public int GetNumberUsersInGroup(int groupId)
        {
            return _repository.GetById(groupId).Users.Count();
        }
        public Group GetByName(string groupName)
        {
            return _repository.GetAll().FirstOrDefault(x=>x.GroupName == groupName);
        }

        //public IdentityUser GetUserById(string id)
        //{
        //    return this.db.Users.FirstOrDefault(n => n.Id == id);
        //}

        public int Create(Group group)
        {
            _repository.Add(group);
            return group.Id;
        }

        public void Delete(Group group)
        {
            _repository.Delete(group);
        }

        //public void AddUserInGroup(int groupId, string userId)
        //{
        //    UsersInGroup userInGroup = new UsersInGroup()
        //    {
        //        GroupId = groupId,
        //        UserId = userId,
        //    };
        //    this.db.UsersInGroup.Add(userInGroup);
        //    this.db.SaveChanges();

        //}

        //public void DeleteUserIntoGroup(int groupId, string userId)
        //{
        //    var user = this.db.UsersInGroup.Where(t => t.GroupId == groupId).FirstOrDefault(t => t.UserId == userId);
        //    this.db.UsersInGroup.Remove(user);
        //    this.db.SaveChanges();
        //}

        //public UsersInGroup GetUserInGroup(int groupId, string userId)
        //{
        //    var user = this.db.UsersInGroup.Where(t => t.GroupId == groupId).FirstOrDefault(t => t.UserId == userId);
        //    return user;
        //}

        public List<Group> GetAll()
        {
            return _repository.GetAll().ToList();
        }

    }

}
    