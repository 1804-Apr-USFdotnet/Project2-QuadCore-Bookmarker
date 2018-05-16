using Bookmarker.Models;
using Bookmarker.Repositories;
using System.Data.Entity;

namespace Bookmarker.Test.Repositories
{
    public class BookmarkerTestContext : DbContext, IDbContext
    {
        public BookmarkerTestContext() : base("BookmarkerTestDb")
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
