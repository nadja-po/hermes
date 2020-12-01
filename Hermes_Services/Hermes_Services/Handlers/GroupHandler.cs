using System.Linq;
using Hermes_Services.Repositories;
using Hermes_Models;
using System.Collections.Generic;

namespace Hermes_Services.Handler
{
    public class GroupHandler
    {
        private GroupRepository _repository;

        public GroupHandler()
        {
            _repository = new GroupRepository();
        }

        public Group GetById(int Id)
        {
            return _repository.GetById(Id);
        }

        public int GetNumberUsersInGroup(int groupId)
        {
            return _repository.GetById(groupId).Users.Count();
        }
        public Group GetByName(string groupName)
        {
            return _repository.GetAll().FirstOrDefault(x=>x.GroupName == groupName);
        }

        public int Create(Group group)
        {
            _repository.Add(group);
            return group.Id;
        }

        public void Delete(Group group)
        {
            _repository.Delete(group);
        }

        public List<Group> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public void ChangeModeratorInGroup(int groupId, string nextModeratorId)
        {
            var group = _repository.GetById(groupId);
            group.ModeratorId = nextModeratorId;
            _repository.Update(group);
        }
    }
}
    