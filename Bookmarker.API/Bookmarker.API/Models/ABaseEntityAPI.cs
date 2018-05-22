using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.API.Models
{
    public class ABaseEntityAPI
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public Dictionary<string, string> Links { get; set; }
    }
}