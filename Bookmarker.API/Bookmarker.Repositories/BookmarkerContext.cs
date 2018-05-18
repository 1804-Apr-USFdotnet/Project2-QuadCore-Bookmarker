using Bookmarker.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Bookmarker.Repositories
{
    public class BookmarkerContext : DbContext, IDbContext
    {
        public BookmarkerContext() : base("BookmarkerDb")
        {

        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Collection> Collections { get; set; }
        public IDbSet<Bookmark> Bookmarks { get; set; }

        DbEntityEntry IDbContext.Entry<T>(T entity)
        {
            return base.Entry(entity);
        }

        IDbSet<T> IDbContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}
