using System.Collections.Generic;
using System.Linq;
using Hermes_Services.Data;
using Hermes_Models;
using Hermes_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes_Services.Repositories
{

    public class GroupRepository : IRepository<Group>
    {
        private readonly ApplicationDbContext _db;

        public GroupRepository() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:hermeschat2020dbserver.database.windows.net,1433;Initial Catalog=Hermes-chat_db;Persist Security Info=False;User ID=hermes.admin;Password=Wasd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            ApplicationDbContext db = new ApplicationDbContext(optionsBuilder.Options);
            _db = db;
        }

        IEnumerable<Group> IRepository<Group>.GetAll => throw new System.NotImplementedException();

        public void Add(Group entity)
        {
            _db.Group.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(Group entity)
        {
            _db.Group.Remove(entity);
            _db.SaveChanges();
        }

        public void Update(Group entity)
        {
            _db.Group.Update(entity);
            _db.SaveChanges();
        }

        public Group GetById(int? Id)
        {
            var result = _db.Group.FirstOrDefault( x => x.Id == Id);
            return result;
        }

        public IEnumerable<Group> GetAll()
        {
            return _db.Group.ToList<Group>();
        }

        public Group GetById(string Id)
        {
            throw new System.NotImplementedException();
        }
    }

}
    