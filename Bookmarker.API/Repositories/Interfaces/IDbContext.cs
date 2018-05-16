using Models.Models;
using System.Data.Entity;

namespace Repositories.Interfaces
{
    public interface IDbContext
    {
        int SaveChanges();

        DbSet<User> Users { get; set; }
        DbSet<Collection> Collections { get; set; }
        DbSet<Bookmark> Bookmarks { get; set; }
    }
}
