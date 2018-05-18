using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookmarker.Models
{
    public class Collection : ABaseEntity, IRatable
    {
        //public Collection(string name, string description, User owner)
        //{
        //    if(owner == null) { owner = new User("", "", ""); }
        //    Id = System.Guid.NewGuid();
        //    Created = System.DateTime.UtcNow;
        //    Name = name;
        //    Description = description;
        //    Private = true;
        //    Rating = 0;
        //    Owner = owner;
        //    owner.Collections.Add(this);
        //    Bookmarks = new HashSet<Bookmark>();
        //}

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool Private { get; set; }
        public int Rating { get; private set; }

        // TODO: Doesn't this need a link to an OwnerId?
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }

        public void ThumbUp()
        {
            Rating++;
        }
    }
}
