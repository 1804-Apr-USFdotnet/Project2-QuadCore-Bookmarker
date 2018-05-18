using Bookmarker.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Bookmarker.Repositories
{
    public interface IDbContext
    {
        int SaveChanges();
        IDbSet<T> Set<T>() where T : ABaseEntity;
        DbEntityEntry Entry<T>(T entity) where T : ABaseEntity;

        IDbSet<User> Users { get; set; }
        IDbSet<Collection> Collections { get; set; }
        IDbSet<Bookmark> Bookmarks { get; set; }
    }
}
