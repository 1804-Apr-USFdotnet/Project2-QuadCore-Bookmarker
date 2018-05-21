
using System;
using System.ComponentModel.DataAnnotations;

namespace Bookmarker.Models
{
    public class Bookmark : ABaseEntity, IRatable
    {
        [Required]
        public string Name { get; set; }

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
