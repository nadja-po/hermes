using Hermes_Models;
using Hermes_Services.Data;
using Hermes_Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Hermes_Services.Repositories
{
    public class GroupManager 
    {
        public GroupManager() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:hermeschat2020dbserver.database.windows.net,1433;Initial Catalog=Hermes-chat_db;Persist Security Info=False;User ID=hermes.admin;Password=Wasd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            ApplicationDbContext db = new ApplicationDbContext(optionsBuilder.Options);
            chatGroup = new GroupRepository(db);
        }

        private GroupRepository chatGroup;

        public List<Group> GetAllGroups()
        {
            return chatGroup.GetAllGroups();
            throw new NotImplementedException();
        }
        public Group Get(int id)
        {
            return chatGroup.Get(id);
            throw new NotImplementedException();
        }

        public int CreateGroup(Group group)
        {
            chatGroup.CreateGroup(group);
            var groupId = group.Id;
            var creatorId = group.CreatorId;
            chatGroup.AddUserInGroup(groupId, creatorId);
            return groupId;
        }

        public void DeleteGroup(int id)
        {
            chatGroup.DeleteGroup(id);
        }

        public List<UsersInGroup> GetUsersByGroup(int groupId)
        {
            return chatGroup.GetUsersByGroup(groupId);
            throw new NotImplementedException();
        }
        public int GetNumberUsersInGroup(int groupId)
        {
            return chatGroup.GetNumberUsersInGroup(groupId);
            throw new NotImplementedException();
        }
        public UsersInGroup GetUserInGroup(int groupId, string userId)
        {
            return chatGroup.GetUserInGroup(groupId, userId);
            throw new NotImplementedException();
        }

        public Group GetByName(string groupName)
        {
            return chatGroup.GetByName(groupName);
        }
        public void AddUserInGroup(int groupId, string userId)
        {
            chatGroup.AddUserInGroup(groupId, userId);
        }

        public void DeleteUserIntoGroup(int groupId, string userId)
        {
            chatGroup.DeleteUserIntoGroup(groupId, userId);
        }

        //public Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
