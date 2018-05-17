using Bookmarker.Models;
using Bookmarker.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Bookmarker.Test
{
    public class BookmarkerTestContext : DbContext, IDbContext
    {
        public class MockDbSet<T> : IDbSet<T> where T : ABaseEntity
        {
            private List<T> _entities;

            public MockDbSet()
            {
                _entities = new List<T>();
            }

            public ObservableCollection<T> Local => null;

            public Expression Expression => _entities.AsQueryable().Expression;

            public Type ElementType => typeof(T);

            public IQueryProvider Provider => _entities.AsQueryable().Provider;

            public T Add(T entity)
            {
                _entities.Add(entity);
                return entity;
            }

            public T Attach(T entity)
            {
                return null;
            }

            public T Create()
            {
                return null;
            }

            public T Find(params object[] keyValues)
            {
                return null;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _entities.GetEnumerator();
            }

            public T Remove(T entity)
            {
                return null;
            }

            TDerivedEntity IDbSet<T>.Create<TDerivedEntity>()
            {
                return null;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _entities.GetEnumerator();
            }
        }

        public BookmarkerTestContext()
        {
            Users = new MockDbSet<User>();
            Collections = new MockDbSet<Collection>();
            Bookmarks = new MockDbSet<Bookmark>();

            User u1 = new User("smith", "password", "smith@mail.com");
            User u2 = new User("frank", "password", "frank@mail.com");
            Collection c1 = new Collection("c#", ".net framework", u1);
            Collection c2 = new Collection("recipes", "my favorites", u2);
            Collection c3 = new Collection("c#", "c# tutorials", u2);
            Bookmark b1 = new Bookmark("c# intro", c1, "csharpintro.com");
            Bookmark b2 = new Bookmark("c# intro", c3, "csharpintro.com");
            Bookmark b3 = new Bookmark("c# keywords", c3, "cskeywords.com");
            Bookmark b4 = new Bookmark("recipes", c2, "food.com");

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

        public IDbSet<User> Users { get; set; }
        public IDbSet<Collection> Collections { get; set; }
        public IDbSet<Bookmark> Bookmarks { get; set; }

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
