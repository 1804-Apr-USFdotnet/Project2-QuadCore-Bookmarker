using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class BookmarkViewModel
    {
        public string Id { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }

        public string Name { get; set; }
        public string URL { get; set; }
        public int Rating { get; set; }

        public CollectionViewModel Collection;
    }
}