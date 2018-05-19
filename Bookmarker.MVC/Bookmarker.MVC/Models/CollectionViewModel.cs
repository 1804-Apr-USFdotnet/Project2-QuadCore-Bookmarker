using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class CollectionViewModel
    {
        public string Guid { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public bool IsPrivate { get; set; }

        public IEnumerable<BookmarkViewModel> Bookmarks;
    }
}