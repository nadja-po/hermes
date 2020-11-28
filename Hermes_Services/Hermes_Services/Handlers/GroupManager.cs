using Hermes_Models;
using Hermes_Services.Handler;
using System.Collections.Generic;

namespace Hermes_Services.Handlers

{
    public class GroupManager
    {
        private GroupHandler _groupHandler;
        private UsersInGroupHandler _userInGroupHandler;

        public GroupManager()
        {
            _groupHandler = new GroupHandler();
            _userInGroupHandler = new UsersInGroupHandler();
        }

        public int CreateGroup(Group group)
        {
            _groupHandler.Create(group);
            _userInGroupHandler.AddUserInGroup(group.Id, group.CreatorId);
            return group.Id;
        }

        public List<UsersInGroup> GetUsersByGroup(int groupId)
        {
            return _userInGroupHandler.GetUsersByGroup(groupId);
        }
    }
}
