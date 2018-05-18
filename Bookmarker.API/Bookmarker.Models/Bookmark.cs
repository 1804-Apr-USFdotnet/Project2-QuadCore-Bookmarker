
using System;
using System.ComponentModel.DataAnnotations;

namespace Bookmarker.Models
{
    public class Bookmark : ABaseEntity, IRatable
    {
        public Bookmark(string name, Collection collection, string url)
        {
            if(collection == null) { collection = new Collection("", "", null); }
            Id = System.Guid.NewGuid();
            Created = System.DateTime.UtcNow;
            Name = name;
            Collection = collection;
            collection.Bookmarks.Add(this);
            URL = url;
            Rating = 0;
        }

        [Required]
        public string Name { get; set; }

        // TODO: Probably needs CollectionId property to link
        public Guid CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string URL { get; set; }
        public int Rating { get; private set; }

        public void ThumbUp()
        {
            Rating++;
        }
    }
}
