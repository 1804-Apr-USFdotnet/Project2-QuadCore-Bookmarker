namespace Bookmarker.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingsubscription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBookmarkRating",
                c => new
                    {
                        UserRefId = c.Guid(nullable: false),
                        BookmarkRefId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserRefId, t.BookmarkRefId })
                .ForeignKey("dbo.Users", t => t.UserRefId, cascadeDelete: true)
                .ForeignKey("dbo.Bookmarks", t => t.BookmarkRefId, cascadeDelete: true)
                .Index(t => t.UserRefId)
                .Index(t => t.BookmarkRefId);
            
            CreateTable(
                "dbo.UserCollectionRating",
                c => new
                    {
                        CollectionRefId = c.Guid(nullable: false),
                        Collection_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CollectionRefId, t.Collection_Id })
                .ForeignKey("dbo.Users", t => t.CollectionRefId, cascadeDelete: true)
                .ForeignKey("dbo.Collections", t => t.Collection_Id, cascadeDelete: true)
                .Index(t => t.CollectionRefId)
                .Index(t => t.Collection_Id);
            
            CreateTable(
                "dbo.UserCollectionSubscriptions",
                c => new
                    {
                        UserRefId = c.Guid(nullable: false),
                        CollectionRefId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserRefId, t.CollectionRefId })
                .ForeignKey("dbo.Users", t => t.UserRefId, cascadeDelete: true)
                .ForeignKey("dbo.Collections", t => t.CollectionRefId, cascadeDelete: true)
                .Index(t => t.UserRefId)
                .Index(t => t.CollectionRefId);

            AddColumn("dbo.Users", "Bookmark_Id", c => c.Guid());
            CreateIndex("dbo.Users", "Bookmark_Id");
            AddForeignKey("dbo.Users", "Bookmark_Id", "dbo.Bookmarks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Bookmark_Id", "dbo.Bookmarks");
            DropForeignKey("dbo.UserCollectionSubscriptions", "CollectionRefId", "dbo.Collections");
            DropForeignKey("dbo.UserCollectionSubscriptions", "UserRefId", "dbo.Users");
            DropForeignKey("dbo.UserCollectionRating", "Collection_Id", "dbo.Collections");
            DropForeignKey("dbo.UserCollectionRating", "CollectionRefId", "dbo.Users");
            DropForeignKey("dbo.UserBookmarkRating", "BookmarkRefId", "dbo.Bookmarks");
            DropForeignKey("dbo.UserBookmarkRating", "UserRefId", "dbo.Users");
            DropIndex("dbo.UserCollectionSubscriptions", new[] { "CollectionRefId" });
            DropIndex("dbo.UserCollectionSubscriptions", new[] { "UserRefId" });
            DropIndex("dbo.UserCollectionRating", new[] { "Collection_Id" });
            DropIndex("dbo.UserCollectionRating", new[] { "CollectionRefId" });
            DropIndex("dbo.UserBookmarkRating", new[] { "BookmarkRefId" });
            DropIndex("dbo.UserBookmarkRating", new[] { "UserRefId" });
            DropIndex("dbo.Users", new[] { "Bookmark_Id" });
            DropColumn("dbo.Users", "Bookmark_Id");
            DropTable("dbo.UserCollectionSubscriptions");
            DropTable("dbo.UserCollectionRating");
            DropTable("dbo.UserBookmarkRating");
        }
    }
}
