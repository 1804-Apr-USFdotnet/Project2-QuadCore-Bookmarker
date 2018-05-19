using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookmarker.Models
{
    public class User : ABaseEntity
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
    }
}
