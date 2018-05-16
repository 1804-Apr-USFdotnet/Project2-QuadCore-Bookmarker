using Bookmarker.Models;
using System.Linq;

namespace Bookmarker.Repositories
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
