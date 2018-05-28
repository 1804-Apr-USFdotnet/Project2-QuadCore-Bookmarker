namespace Bookmarker.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatingSubscription : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Collections", "OwnerId", "dbo.Users");
            AddColumn("dbo.Collections", "User_Id", c => c.Guid());
            AddColumn("dbo.Users", "BookmarkRating_Id", c => c.Guid());
            AddColumn("dbo.Users", "CollectionRating_Id", c => c.Guid());
            AddColumn("dbo.Users", "CollectionSubscriptions_Id", c => c.Guid());
            AddColumn("dbo.Users", "Collection_Id", c => c.Guid());
            AddColumn("dbo.Users", "Collection_Id1", c => c.Guid());
            AddColumn("dbo.Users", "Bookmark_Id", c => c.Guid());
            AddColumn("dbo.Users", "Bookmark_Id1", c => c.Guid());
            CreateIndex("dbo.Collections", "User_Id");
            CreateIndex("dbo.Users", "BookmarkRating_Id");
            CreateIndex("dbo.Users", "CollectionRating_Id");
            CreateIndex("dbo.Users", "CollectionSubscriptions_Id");
            CreateIndex("dbo.Users", "Collection_Id");
            CreateIndex("dbo.Users", "Collection_Id1");
            CreateIndex("dbo.Users", "Bookmark_Id");
            CreateIndex("dbo.Users", "Bookmark_Id1");
            AddForeignKey("dbo.Users", "BookmarkRating_Id", "dbo.Bookmarks", "Id");
            AddForeignKey("dbo.Users", "CollectionRating_Id", "dbo.Collections", "Id");
            AddForeignKey("dbo.Users", "CollectionSubscriptions_Id", "dbo.Collections", "Id");
            AddForeignKey("dbo.Users", "Collection_Id", "dbo.Collections", "Id");
            AddForeignKey("dbo.Users", "Collection_Id1", "dbo.Collections", "Id");
            AddForeignKey("dbo.Users", "Bookmark_Id", "dbo.Bookmarks", "Id");
            AddForeignKey("dbo.Users", "Bookmark_Id1", "dbo.Bookmarks", "Id");
            AddForeignKey("dbo.Collections", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Collections", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Bookmark_Id1", "dbo.Bookmarks");
            DropForeignKey("dbo.Users", "Bookmark_Id", "dbo.Bookmarks");
            DropForeignKey("dbo.Users", "Collection_Id1", "dbo.Collections");
            DropForeignKey("dbo.Users", "Collection_Id", "dbo.Collections");
            DropForeignKey("dbo.Users", "CollectionSubscriptions_Id", "dbo.Collections");
            DropForeignKey("dbo.Users", "CollectionRating_Id", "dbo.Collections");
            DropForeignKey("dbo.Users", "BookmarkRating_Id", "dbo.Bookmarks");
            DropIndex("dbo.Users", new[] { "Bookmark_Id1" });
            DropIndex("dbo.Users", new[] { "Bookmark_Id" });
            DropIndex("dbo.Users", new[] { "Collection_Id1" });
            DropIndex("dbo.Users", new[] { "Collection_Id" });
            DropIndex("dbo.Users", new[] { "CollectionSubscriptions_Id" });
            DropIndex("dbo.Users", new[] { "CollectionRating_Id" });
            DropIndex("dbo.Users", new[] { "BookmarkRating_Id" });
            DropIndex("dbo.Collections", new[] { "User_Id" });
            DropColumn("dbo.Users", "Bookmark_Id1");
            DropColumn("dbo.Users", "Bookmark_Id");
            DropColumn("dbo.Users", "Collection_Id1");
            DropColumn("dbo.Users", "Collection_Id");
            DropColumn("dbo.Users", "CollectionSubscriptions_Id");
            DropColumn("dbo.Users", "CollectionRating_Id");
            DropColumn("dbo.Users", "BookmarkRating_Id");
            DropColumn("dbo.Collections", "User_Id");
            AddForeignKey("dbo.Collections", "OwnerId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
