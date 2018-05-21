using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookmarker.Models
{
    public class Collection : ABaseEntity, IRatable
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool Private { get; set; }
        public int Rating { get; set; }

        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }

        public void ThumbUp()
        {
            Rating++;
        }
    }
}
