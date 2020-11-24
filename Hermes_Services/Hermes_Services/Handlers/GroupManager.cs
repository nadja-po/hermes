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
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer("Server=tcp:hermeschat2020dbserver.database.windows.net,1433;Initial Catalog=Hermes-chat_db;Persist Security Info=False;User ID=hermes.admin;Password=Wasd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                ApplicationDbContext db = new ApplicationDbContext(optionsBuilder.Options);
            _groupHandler = new GroupHandler(db);
            _userInGroupHandler = new UsersInGroupHandler(db);

        }

        public IEnumerable<Group> GetAllGroups()
        {
            return _groupHandler.GetAll();
        }
        public Group Get(int id)
        {
            return _groupHandler.GetById(id);
        }

        public int CreateGroup(Group group)
        {
            _groupHandler.Create(group);
            _userInGroupHandler.AddUserInGroup(group.Id, group.CreatorId);
            return group.Id;
        }

        public void Delete(Group group)
        {
            _groupHandler.Delete(group);
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
    }
}
