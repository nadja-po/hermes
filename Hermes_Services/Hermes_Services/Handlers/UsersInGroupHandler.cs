using System.Linq;
using Hermes_Services.Repositories;
using Hermes_Models;
using System.Collections.Generic;

namespace Hermes_Services.Handler
{

    public class UsersInGroupHandler
    {
        private UsersInGroupRepository _repository;

        public UsersInGroupHandler()
        {
            _repository = new UsersInGroupRepository();
        }

        public int AddUserInGroup(Group group, string userId)
        {
            UsersInGroup userInGroup = new UsersInGroup()
            {
                GroupId = group.Id,
                UserId = userId,
            };

            _repository.Add(userInGroup);
            return group.Id;
        }

        public int CreateUserInGroup(Group group, string userId)
        {
            UsersInGroup userInGroup = new UsersInGroup()
            {
                Group = group,
                UserId = userId,
            };

            _repository.Add(userInGroup);
            return group.Id;
        }
        public int Create(UsersInGroup userInGroup)
        {
            _repository.Add(userInGroup);
            return userInGroup.Id;
        }

        public void Delete(UsersInGroup userInGroup)
        {
            _repository.Delete(userInGroup);
        }

        public void DeleteUserIntoGroup(int groupId, string userId)
        {
            var userInGroup = _repository.GetAll().Where(t => t.GroupId == groupId).FirstOrDefault(t => t.UserId == userId);
            _repository.Delete(userInGroup);
        }

        public List<UsersInGroup> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public List<UsersInGroup> GetGroupByUser(string userId)
        {
            return _repository.GetAll().Where(x => x.UserId == userId).ToList();
        }

        public List<UsersInGroup> GetUsersByGroup(int groupId)
        {
            return _repository.GetAll().Where(x => x.GroupId == groupId).ToList();
        }

        public UsersInGroup GetUserInGroupById(int id)
        {
            return _repository.GetById(id);
        }

        public UsersInGroup GetUserInGroup(int groupId, string userId)
        {
            return _repository.GetAll().Where(t => t.GroupId == groupId).FirstOrDefault(t => t.UserId == userId);
        }

        public string GetId(UsersInGroup userInGroup)
        {
            return _repository.GetId(userInGroup);
        }

        public IEnumerable<UsersInGroup> GetAllBanned(int groupId)
        {
            return _repository.GetAllBanned(groupId);
        }
    }
}
