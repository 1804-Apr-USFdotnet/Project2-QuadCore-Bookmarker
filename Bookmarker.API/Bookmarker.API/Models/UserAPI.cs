using Bookmarker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.API.Models
{
    public class UserAPI
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public string MyCollectionsURI { get; set; }

        public UserAPI(User user)
        {
            this.Id = user.Id;
            this.Created = user.Created;
            this.Modified = user.Modified;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Email = user.Email;
            this.MyCollectionsURI = $"api/Users/{this.Id}/Collections";
        }
    }
}