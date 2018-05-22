using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class UserAPI
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public Dictionary<string, string> Links { get; set; }
    }
}