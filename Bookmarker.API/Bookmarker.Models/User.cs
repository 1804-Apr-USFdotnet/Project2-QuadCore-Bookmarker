using System.Collections.Generic;

namespace Bookmarker.Models
{
    public class User : ABaseEntity
    {
        public User()
        {
            Id = System.Guid.NewGuid();
            Created = System.DateTime.UtcNow;
            Collections = new HashSet<Collection>();
        }

        public User(string username, string password, string email)
        {
            Id = System.Guid.NewGuid();
            Created = System.DateTime.UtcNow;
            Username = username;
            Password = password;
            Email = email;
            Collections = new HashSet<Collection>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
    }
}
