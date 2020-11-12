using Hermes_chat.Data;
using Hermes_chat.Models;
using Hermes_chat.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes_chat.Managers
{
    public class GroupManager 
    {
        public GroupManager() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:hermes2020dbserver.database.windows.net,1433;Initial Catalog=Hermes-chat_db;Persist Security Info=False;User ID=agnese.brauna;Password=Wasd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
            var id = group.Id;
            return id;
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
