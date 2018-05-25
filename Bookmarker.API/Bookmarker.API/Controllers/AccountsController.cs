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
using System.Threading.Tasks;
using System.Web.Http;
using Bookmarker.Repositories;
using Bookmarker.Models;
using System.Web.Http.Cors;
using NLog;

namespace Bookmarker.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AccountsController : ApiController
    {
        [HttpGet]
        [Route("~/api/Accounts/WhoAmI")]
        [AllowAnonymous]
        public IHttpActionResult WhoAmI()
        {
            string name = System.Web.HttpContext.Current?.User?.Identity?.GetUserName();
            if(name==null) { return BadRequest(); }

            Repository<User> userRepo = new Repository<User>(new BookmarkerContext());
            User user = userRepo.Table.First(x => x.Username == name);
            
            return Ok(new UserAPI(user));
        }

        [HttpGet]
        [Route("~/api/Accounts/LoggedIn")]
        [AllowAnonymous]
        public IHttpActionResult LoggedIn()
        {
            return Ok(System.Web.HttpContext.Current.User?.Identity.IsAuthenticated ?? false);
        }

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
            return Ok();
        }

        [HttpPost]
        [Route("~/api/Accounts/Login")]
        [AllowAnonymous]
        public IHttpActionResult Login(Account account)
        {
            Logger logger = LogManager.GetLogger("file");
            logger.Log(LogLevel.Info, "Start");

            if (!ModelState.IsValid)
            {
                logger.Log(LogLevel.Info, "Model state invalid");
                return BadRequest();
            }

            // Login
            logger.Log(LogLevel.Info, "init user store");
            logger.Log(LogLevel.Info, $"username: {account.Username}");
            var userStore = new UserStore<IdentityUser>(new AccountDbContext());
            logger.Log(LogLevel.Info, $"userStore num users: {userStore.Users.Count()}");
            var userManager = new UserManager<IdentityUser>(userStore);
            logger.Log(LogLevel.Info, $"user manager initialized");
            var user = userManager.Users.FirstOrDefault(u => u.UserName == account.Username);
            logger.Log(LogLevel.Info, $"user is {user.UserName}");

            if (user == null)
            {
                logger.Log(LogLevel.Info, "user is null");
                return BadRequest();
            }

            if (!userManager.CheckPassword(user, account.Password))
            {
                logger.Log(LogLevel.Info, "password unauthorized");
                return Unauthorized();
            }

            logger.Log(LogLevel.Info, "create identity claim");
            var authManager = Request.GetOwinContext().Authentication;
            var claimsIdentity = userManager.CreateIdentity(user, WebApiConfig.AuthenticationType);

            logger.Log(LogLevel.Info, "sign in");
            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);

            logger.Log(LogLevel.Info, "return");
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
