using System.Collections.Generic;
using System.Linq;
using Hermes_Services.Data;
using Hermes_Models;
using Hermes_Interfaces;

namespace Hermes_Services.Repositories
{

    public class UsersInGroupRepository : IRepository<UsersInGroup>
    {
        private readonly ApplicationDbContext _db;

        public UsersInGroupRepository(ApplicationDbContext db)
        {
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
    }
}
    