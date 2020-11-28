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
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Hermes_chat-76EDEDF5-906A-401D-98D8-1121EADE40D8;Trusted_Connection=True;MultipleActiveResultSets=true");
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
    