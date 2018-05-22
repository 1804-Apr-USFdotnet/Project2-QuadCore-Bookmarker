using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookmarker.Models
{
    public class User : ABaseEntity
    {
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(250)]
        public string Email { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
    }
}
