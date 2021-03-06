﻿using Bookmarker.Models;
using System.Collections.Generic;
using System.Configuration;

namespace Bookmarker.API.Models
{
    public class UserAPI : ABaseEntityAPI
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public UserAPI(User user)
        {
            if (user == null) { user = new User(); }
            this.Id = user.Id;
            this.Created = user.Created;
            this.Modified = user.Modified;
            this.Username = user.Username;
            this.Email = user.Email;
            string apiDomain = ConfigurationManager.AppSettings.Get("ServiceUri");
            Links = new Dictionary<string, string>
            {
                { "self", $"{apiDomain}/Users/{this.Id}" },
                { "my_collections", $"{apiDomain}/Users/{this.Id}/Collections" },
                { "users", $"{apiDomain}/Users" },
                { "collections", $"{apiDomain}/Collections" }
            };
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
    }
}