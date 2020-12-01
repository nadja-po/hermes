using System.Collections.Generic;

namespace Hermes_Interfaces
{
    public interface IRepository<T> where T : TEntity
    {
        IEnumerable<T> GetAll { get; }
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(int? Id);
        T GetById(string Id);
    }
}
