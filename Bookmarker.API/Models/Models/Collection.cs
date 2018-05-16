using Models.Abstracts;
using System;
using System.Collections.Generic;

namespace Models.Models
{
    public class Collection : ABaseEntity
    {
        public Collection(string name, string description, User owner)
        {
            Name = name;
            Description = description;
            Private = true;
            Rating = 0;
            Owner = owner;
            Bookmarks = new HashSet<Bookmark>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public int Rating { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
    }
}
