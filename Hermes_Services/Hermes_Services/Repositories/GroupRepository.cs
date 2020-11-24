using System.Collections.Generic;
using System.Linq;
using Hermes_Services.Data;
using Hermes_Models;
using Hermes_Interfaces;

namespace Hermes_Services.Repositories
{

    public class GroupRepository : IRepository<Group>
    {
        private readonly ApplicationDbContext _db;

        public GroupRepository(ApplicationDbContext db) 
        {
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
            var result = _db.Group.ToList<Group>();
            if (result.Count() > 0)
            {
                return result;
            }
            else
            {
                return new List<Group>(); 
            }
        }

        public Group GetById(string Id)
        {
            throw new System.NotImplementedException();
        }
    }
}
    