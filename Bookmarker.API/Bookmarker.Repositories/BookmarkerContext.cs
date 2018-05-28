using Bookmarker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany<Collection>(u => u.CollectionRating)
                .WithMany(c => c.RatedUsers)
                .Map(cu =>
                {
                    cu.MapLeftKey("UserRefId");
                    cu.MapLeftKey("CollectionRefId");
                    cu.ToTable("UserCollectionRating");
                });

            modelBuilder.Entity<User>()
                .HasOptional(u => u.CollectionRating)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany<Bookmark>(u => u.BookmarkRating)
                .WithMany(b => b.RatedUsers)
                .Map(bu =>
                {
                    bu.MapLeftKey("UserRefId");
                    bu.MapRightKey("BookmarkRefId");
                    bu.ToTable("UserBookmarkRating");
                });

            modelBuilder.Entity<User>()
                .HasOptional(u => u.BookmarkRating)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany<Collection>(u => u.CollectionSubscriptions)
                .WithMany(c => c.SubscribedUsers)
                .Map( cuSub =>
                {
                    cuSub.MapLeftKey("UserRefId");
                    cuSub.MapRightKey("CollectionRefId");
                    cuSub.ToTable("UserCollectionSubscriptions");
                });

            modelBuilder.Entity<User>()
                .HasOptional(u => u.CollectionSubscriptions)
                .WithMany()
                .WillCascadeOnDelete(false);
        }

        public override int SaveChanges()
        {
            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            AddedEntities.ForEach(E =>
            {
                E.Property("Id").CurrentValue = Guid.NewGuid();
                E.Property("Created").CurrentValue = DateTime.Now;
            });

            var ModifiedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            ModifiedEntities.ForEach(E =>
            {
                E.Property("Modified").CurrentValue = DateTime.Now;
            });

            return base.SaveChanges();
        }

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
