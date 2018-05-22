﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage =
            "Password must be between 8 and 40 characters inclusive.")]
        public string Password { get; set; }
        public string Email { get; set; }

        public IEnumerable<CollectionViewModel> Collections { get; set; }
    }
}