using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class BookmarkViewModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public string Name { get; set; }
        public string URL { get; set; }
        public int Rating { get; set; }

        public CollectionViewModel Collection;
        public Guid CollectionId;
    }
}