using Models.Models;
using Repositories.Interfaces;
using System.Data.Entity;

namespace Repositories.Repositories
{
    public class BookmarkerContext : DbContext, IDbContext
    {
        public BookmarkerContext() : base("BookmarkerDb")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }

        IDbSet<T> IDbContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}
