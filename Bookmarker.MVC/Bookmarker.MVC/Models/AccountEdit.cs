using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class AccountEdit : AccountViewModel
    {
        [Required]
        public string newUsername { get; set; }
        public string newEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage =
            "Password must be between 8 and 40 characters inclusive.")]
        public string newPassword { get; set; }
    }
}