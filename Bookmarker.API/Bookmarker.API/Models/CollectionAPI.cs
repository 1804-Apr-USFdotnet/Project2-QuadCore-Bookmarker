﻿using Bookmarker.Models;
using Bookmarker.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Bookmarker.API.Models
{
    public class CollectionAPI : ABaseEntityAPI
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public int Rating { get; set; }
        public Guid OwnerId { get; set; }

        public CollectionAPI(Collection coll)
        {
            if (coll == null) { coll = new Collection(); }
            this.Id = coll.Id;
            this.Created = coll.Created;
            this.Modified = coll.Modified;
            this.Name = coll.Name;
            this.Description = coll.Description;
            this.Private = coll.Private;
            this.Rating = coll.Rating;
            this.OwnerId = coll.OwnerId;
            string apiDomain = ConfigurationManager.AppSettings.Get("ServiceUri");
            Links = new Dictionary<string, string>();
            Links.Add("self", $"{apiDomain}/Collections/{this.Id}");
            Links.Add("owner", $"{apiDomain}/Users/{coll.OwnerId}");
            Links.Add("my_bookmarks", $"{apiDomain}/Collections/{this.Id}/Bookmarks");
            Links.Add("collections", $"{apiDomain}/Collections");
            Links.Add("users", $"{apiDomain}/Users");
        }

        public Collection ToCollectionNoBookmarks()
        {
            return new Collection()
            {
                Id = this.Id,
                Created = this.Created,
                Modified = this.Modified,
                Name = this.Name,
                Description = this.Description,
                Private = this.Private,
                Rating = this.Rating,
                OwnerId = this.OwnerId,
                Bookmarks = null
            };
        }
    }
}