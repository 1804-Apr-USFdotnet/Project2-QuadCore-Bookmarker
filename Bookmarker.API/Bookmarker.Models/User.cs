using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookmarker.Models
{
    public class User : ABaseEntity
    {
        public User(string username, string password, string email)
        {
            Id = System.Guid.NewGuid();
            Created = System.DateTime.UtcNow;
            Username = username;
            Password = password;
            Email = email;
            Collections = new HashSet<Collection>();
        }
        
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<Collection> Collections { get; set; }
    }
}
