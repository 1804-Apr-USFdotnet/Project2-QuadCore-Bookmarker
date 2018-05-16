using Models.Abstracts;
using System.Linq;

namespace Repositories.Interfaces
{
    public interface IRepository<T> where T : ABaseEntity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
