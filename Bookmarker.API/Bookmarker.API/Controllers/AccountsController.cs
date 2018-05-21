using Bookmarker.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
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
        [Route("~/api/Accounts/Register")]
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
            Login(account);

            return Ok();
        }

        [HttpPost]
        [Route("~/api/Accounts/Login")]
        [AllowAnonymous]
        public IHttpActionResult Login(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Login
            var userStore = new UserStore<IdentityUser>(new AccountDbContext());
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Users.FirstOrDefault(u => u.UserName == account.Username);

            if (user == null)
            {
                return BadRequest();
            }

            if (!userManager.CheckPassword(user, account.Password))
            {
                return Unauthorized();
            }

            var authManager = Request.GetOwinContext().Authentication;
            var claimsIdentity = userManager.CreateIdentity(user, WebApiConfig.AuthenticationType);

            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);

            return Ok();
        }

        [HttpGet]
        [Route("~/api/Accounts/Logout")]
        public IHttpActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(WebApiConfig.AuthenticationType);
            return Ok();
        }
    }
}
