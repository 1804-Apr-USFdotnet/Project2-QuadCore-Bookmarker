using System.Collections.Generic;

namespace Models
{
    public class User : ABaseEntity
    {
        public User(string username, string password, string email)
        {
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
