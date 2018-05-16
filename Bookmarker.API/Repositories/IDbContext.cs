using Models;
using System.Data.Entity;

namespace Repositories
{
    public interface IDbContext
    {
        int SaveChanges();
        IDbSet<T> Set<T>() where T : ABaseEntity;

        DbSet<User> Users { get; set; }
        DbSet<Collection> Collections { get; set; }
        DbSet<Bookmark> Bookmarks { get; set; }
    }
}
