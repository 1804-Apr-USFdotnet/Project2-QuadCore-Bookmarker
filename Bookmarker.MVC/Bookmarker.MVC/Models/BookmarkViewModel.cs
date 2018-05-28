using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class BookmarkViewModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string URL { get; set; }
        public int Rating { get; set; }

        public virtual CollectionViewModel Collection { get; set; }
        public Guid CollectionId { get; set; }
    }
}