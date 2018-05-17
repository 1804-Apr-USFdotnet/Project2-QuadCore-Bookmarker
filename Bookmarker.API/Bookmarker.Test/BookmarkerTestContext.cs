using Bookmarker.Models;
using Bookmarker.Repositories;
using System.Data.Entity;

namespace Bookmarker.Test
{
    public class BookmarkerTestContext : DbContext, IDbContext
    {
        public BookmarkerTestContext()
        {
            Users.Add(new User("smith", "password", "smith@mail.com"));
            Users.Add(new User("frank", "password", "frank@mail.com"));
            Collections.Add(new Collection("c#", ".net framework", Users.SingleAsync(x=>x.Username=="smith").Result));
            Collections.Add(new Collection("recipes", "my favorites", Users.SingleAsync(x=>x.Username=="smith").Result));
            Collections.Add(new Collection("c#", "c# tutorials", Users.SingleAsync(x=>x.Username=="frank").Result));
            Bookmarks.Add(new Bookmark("c# intro", Collections.SingleAsync(x => x.Description == "c# tutorials").Result, "csharpintro.com"));
            Bookmarks.Add(new Bookmark("c# keywords", Collections.SingleAsync(x => x.Description == "c# tutorials").Result, "csharpkeywords.com"));
            Bookmarks.Add(new Bookmark(".net overview", Collections.SingleAsync(x => x.Description == "c# tutorials").Result, "dotnet.com"));
            Bookmarks.Add(new Bookmark(".net summary", Collections.SingleAsync(x => x.Description == ".net framework").Result, "dotnet.com"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }

        public override int SaveChanges()
        {
            return -1;
        }

        IDbSet<T> IDbContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}
