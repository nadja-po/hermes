using System.Collections.Generic;
using System.Linq;
using Hermes_Services.Data;
using Hermes_Models;
using Hermes_Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Hermes_Services.Repositories
{

    public class UsersInGroupRepository : IRepository<UsersInGroup>
    {
        private readonly ApplicationDbContext _db;


        public UsersInGroupRepository()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:hermeschat2020dbserver.database.windows.net,1433;Initial Catalog=Hermes-chat_db;Persist Security Info=False;User ID=hermes.admin;Password=Wasd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            ApplicationDbContext db = new ApplicationDbContext(optionsBuilder.Options);
            _db = db;
        }

        IEnumerable<UsersInGroup> IRepository<UsersInGroup>.GetAll => throw new System.NotImplementedException();

        public void Add(UsersInGroup entity)
        {

            _db.UsersInGroup.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(UsersInGroup entity)
        {
            _db.UsersInGroup.Remove(entity);
            _db.SaveChanges();
        }

        public void Update(UsersInGroup entity)
        {
            _db.UsersInGroup.Update(entity);
            _db.SaveChanges();
        }

        public UsersInGroup GetById(int? Id)
        {
            var result = _db.UsersInGroup.FirstOrDefault( x => x.Id == Id);
            return result;
        }

        public IEnumerable<UsersInGroup> GetAll()
        {
            var result = _db.UsersInGroup.ToList<UsersInGroup>();
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<UsersInGroup>(); 
            }
        }

        public UsersInGroup GetById(string Id)
        {
            throw new System.NotImplementedException();
        }

        public string GetId(UsersInGroup userInGroup)
        {
            var result = _db.UsersInGroup.FirstOrDefault(x => x == userInGroup).UserId;
            return result;
        }

       
    }
}
    