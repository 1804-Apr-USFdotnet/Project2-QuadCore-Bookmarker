using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookmarker.API.Models
{
    public class AccountEdit : Account
    {
        public string newUsername { get; set; }
        public string newEmail { get; set; }
        public string newPassword { get; set; }
    }
}