using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class AccountViewModel
    {
        public string Guid { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public IEnumerable<CollectionViewModel> Collections { get; set; }
    }
}