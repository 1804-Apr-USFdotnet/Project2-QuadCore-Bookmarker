using Bookmarker.Models;
using Bookmarker.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Bookmarker.Test
{
    public class MockDbSet<T> : IDbSet<T> where T : ABaseEntity
    {
        private readonly List<T> _entities;

        public MockDbSet()
        {
            _entities = new List<T>();
        }

        public ObservableCollection<T> Local => new ObservableCollection<T>(_entities);

        public Expression Expression => _entities.AsQueryable().Expression;

        public Type ElementType => _entities.AsQueryable().ElementType;

        public IQueryProvider Provider => _entities.AsQueryable().Provider;

        public T Add(T entity)
        {
            _entities.Add(entity);
            return entity;
        }

        public T Attach(T entity)
        {
            _entities.Add(entity);
            return entity;
        }

        public T Create()
        {
            throw new NotImplementedException();
        }

        public T Find(params object[] keyValues)
        {
            return _entities.First(x => x.Id == (Guid) keyValues[0]);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        public T Remove(T entity)
        {
            _entities.Remove(entity);
            return entity;
        }

        TDerivedEntity IDbSet<T>.Create<TDerivedEntity>()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }
    }

    public class BookmarkerTestContext : DbContext, IDbContext
    {

        public BookmarkerTestContext()
        {
            Users = new MockDbSet<User>();
            Collections = new MockDbSet<Collection>();
            Bookmarks = new MockDbSet<Bookmark>();

            User u1 = new User();
            u1.Username = "smith";
            u1.Password = "password";
            u1.Email = "smith@mail.com";
            User u2 = new User();
            u2.Username = "frank";
            u2.Password = "password";
            u2.Email = "frank@mail.com";
            Collection c1 = new Collection();
            c1.Name = "c#";
            c1.Description = ".net framework";
            c1.Owner = u1;
            Collection c2 = new Collection();
            c2.Name = "recipes";
            c2.Description = "my favorites";
            c2.Owner = u2;
            Collection c3 = new Collection();
            c3.Name = "c#";
            c3.Description = "c# tutorials";
            c3.Owner = u2;
            Bookmark b1 = new Bookmark();
            b1.Name = "c# intro";
            b1.Collection = c1;
            b1.URL = "csharpintro.com";
            Bookmark b2 = new Bookmark();
            b2.Name = "c# intro";
            b2.Collection = c3;
            b2.URL = "csharpintro.com";
            Bookmark b3 = new Bookmark();
            b3.Name = "c# keywords";
            b3.Collection = c3;
            b3.URL = "cskeywords.com";
            Bookmark b4 = new Bookmark();
            b4.Name = "recipes";
            b4.Collection = c2;
            b4.URL = "food.com";

            Users.Add(u1);
            Users.Add(u2);
            Collections.Add(c1);
            Collections.Add(c2);
            Collections.Add(c3);
            Bookmarks.Add(b1);
            Bookmarks.Add(b2);
            Bookmarks.Add(b3);
            Bookmarks.Add(b4);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BookmarkerTestContext>(null);
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Collection> Collections { get; set; }
        public IDbSet<Bookmark> Bookmarks { get; set; }

        DbEntityEntry IDbContext.Entry<T>(T entity)
        {
            return base.Entry(entity);
        }

        public override int SaveChanges()
        {
            return -1;
        }

        IDbSet<T> IDbContext.Set<T>()
        {
            if(typeof(T) == typeof(User))
            {
                return (IDbSet<T>) Users;
            }
            if(typeof(T) == typeof(Collection))
            {
                return (IDbSet<T>) Collections;
            }
            if(typeof(T) == typeof(Bookmark))
            {
                return (IDbSet<T>) Bookmarks;
            }
            else
            {
                return null;
            }
        }
    }
}
