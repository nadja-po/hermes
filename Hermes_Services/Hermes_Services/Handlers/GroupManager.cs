using Hermes_Models;
using Hermes_Services.Data;
using Hermes_Services.Handler;
using Microsoft.EntityFrameworkCore;
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
        public int GetNumberUsersInGroup(int groupId)
        {
            return _groupHandler.GetNumberUsersInGroup(groupId);
        }
        public UsersInGroup GetUserInGroup(int groupId, string userId)
        {
            return _userInGroupHandler.GetUserInGroup(groupId, userId);
        }

        public Group GetByName(string groupName)
        {
            return _groupHandler.GetByName(groupName);
        }
        public void AddUserInGroup(int groupId, string userId)
        {
            _userInGroupHandler.AddUserInGroup(groupId, userId);
        }

        public void DeleteUserIntoGroup(int groupId, string userId)
        {
            _userInGroupHandler.DeleteUserIntoGroup(groupId, userId);
        }

        //public Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

       

        public void ChangeModeratorInGroup(int groupId, string nextModeratorId)
        {
            _groupHandler.ChangeModeratorInGroup(groupId, nextModeratorId);
        }

        public UsersInGroup GetUserInGroupBiId(int id)
        {
            return _userInGroupHandler.GetUserInGroupBiId(id);            
        }
    }
}
