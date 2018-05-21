using Bookmarker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Configuration;

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
        public Dictionary<string, string> Links { get; set;}

        public UserAPI(User user)
        {
            this.Id = user.Id;
            this.Created = user.Created;
            this.Modified = user.Modified;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Email = user.Email;
            string apiDomain = ConfigurationManager.AppSettings.Get("ServiceDomain");
            Links = new Dictionary<string, string>();
            Links.Add("self", $"{apiDomain}/Users/{this.Id}");
            Links.Add("my_collections", $"{apiDomain}/Users/{this.Id}/Collections");
            Links.Add("users", $"{apiDomain}/Users");
            Links.Add("collections", $"{apiDomain}/Collections");
        }
    }
}