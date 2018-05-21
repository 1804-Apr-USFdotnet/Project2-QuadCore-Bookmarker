using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookmarker.API.Models
{
    public class Account
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = 
            "Password must be between 8 and 40 characters inclusive.")]
        public string Password { get; set; }
    }
}