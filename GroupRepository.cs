using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes_Services.Data;
using Hermes_Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Hermes_chat.Repositories
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
            using (var db = this.db)
            {
                return db.Group.FirstOrDefault(n => n.Id == id);
            }

        }
       
        public List<Group> GetAllGroups()
        {
            using (var db = this.db)
            {
                return db.Group.ToList();
            }

        }
        public List<UsersInGroup> GetUsersByGroup(int groupId)
        {
            using (var db = this.db)
            {
                return db.UsersInGroup.Where(n => n.GroupId == groupId).ToList();
            }

        }

        public Group GetByName(string groupName)
        {
            using (var db = this.db)
            {
                return db.Group.FirstOrDefault(n => n.GroupName == groupName);
            }

        }

        public int CreateGroup(Group group)
        {
            using (var db = this.db)
            {
                db.Group.Add(group);
                db.SaveChanges();
                var id = group.Id;
                return id;
            }
        }

        public void DeleteGroup(int id)
        {
            using (var db = this.db)
            {
                var group = db.Group.FirstOrDefault(t => t.Id == id);
                db.Group.Remove(group);

                db.SaveChanges();
            }
        }

    }
}
    