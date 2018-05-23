using Bookmarker.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Bookmarker.API.Models
{
    public class BookmarkAPI : ABaseEntityAPI
    {
        public string Name { get; set; }
        public Guid CollectionId { get; set; }
        public virtual Collection Collection { get; set; }
        public string URL { get; set; }
        public int Rating { get; private set; }

        public BookmarkAPI(Bookmark bookmark)
        {
            if (bookmark == null) { bookmark = new Bookmark(); }

            this.Id = bookmark.Id;
            this.Created = bookmark.Created;
            this.Modified = bookmark.Modified;

            this.Name = bookmark.Name;
            this.CollectionId = bookmark.CollectionId;
            this.URL = bookmark.URL;
            this.Rating = bookmark.Rating;

            string apiDomain = ConfigurationManager.AppSettings.Get("ServiceUri");
            Links = new Dictionary<string, string>
            {
                { "self", $"{apiDomain}/Bookmarks/{this.Id}" },
                { "parent_collection", $"{apiDomain}/Collections/{this.CollectionId}" },
                { "bookmarks", $"{apiDomain}/Bookmarks" },
            };
        }

        public Bookmark ToBookmarkNoCollection()
        {
            return new Bookmark()
            {
                Id = this.Id,
                Created = this.Created,
                Modified = this.Modified,
                Name = this.Name,
                CollectionId = this.CollectionId,
                Collection = null,
                URL = this.URL
            };
        }
    }
}