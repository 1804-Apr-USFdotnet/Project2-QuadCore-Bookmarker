using Bookmarker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Configuration;
using Bookmarker.Repositories;

namespace Bookmarker.API.Models
{
    public class UserAPI
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public Dictionary<string, string> Links { get; set;}

        public UserAPI(User user)
        {
            if (user == null) { user = new User(); }
            this.Id = user.Id;
            this.Created = user.Created;
            this.Modified = user.Modified;
            this.Username = user.Username;
            this.Email = user.Email;
            string apiDomain = ConfigurationManager.AppSettings.Get("ServiceUri");
            Links = new Dictionary<string, string>();
            Links.Add("self", $"{apiDomain}/Users/{this.Id}");
            Links.Add("my_collections", $"{apiDomain}/Users/{this.Id}/Collections");
            Links.Add("users", $"{apiDomain}/Users");
            Links.Add("collections", $"{apiDomain}/Collections");
        }

        public User ToUserNoCollections()
        {
            return new User()
            {
                Id = this.Id,
                Created = this.Created,
                Modified = this.Modified,
                Username = this.Username,
                Email = this.Email,
                Collections = null
            };
        }

        public ICollection<Collection> GetCollectionsByUserId(Guid Id)
        {
            Repository<User> userRepository =
                new Repository<User>(new BookmarkerContext());

            return userRepository.GetById(Id).Collections;
        }
    }
}