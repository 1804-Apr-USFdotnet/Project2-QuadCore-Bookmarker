using Models.Models;
using Repositories.Interfaces;
using System.Data.Entity;

namespace Repositories.Repositories
{
    public class BookmarkerContext : DbContext, IDbContext
    {
        public BookmarkerContext()
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
    }
}
