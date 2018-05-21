using Bookmarker.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bookmarker.API.Controllers
{
    public class AccountsController : ApiController
    {
        [HttpPost]
        [Route("~/api/Register")]
        [AllowAnonymous]
        public IHttpActionResult Register(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Register
            var userStore = new UserStore<IdentityUser>(new AccountDbContext());
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = new IdentityUser(account.Username);

            if (userManager.Users.Any(u => u.UserName == account.Username))
            {
                return BadRequest();
            }

            userManager.Create(user, account.Password);

            // Login

            return Ok();
        }
    }
}
