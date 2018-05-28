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
            Logout();
            Login(account);
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
            var context = new AccountDbContext();
            logger.Log(LogLevel.Info, $"context null?: {context == null}");
            logger.Log(LogLevel.Info, $"context db connection state: {context.Database.Connection.State}");
            logger.Log(LogLevel.Info, $"context db connection string: {context.Database.Connection.ConnectionString}");
            logger.Log(LogLevel.Info, $"context db connection db name: {context.Database.Connection.Database}");

            UserStore<IdentityUser> userStore = null;
            try
            {
                userStore = new UserStore<IdentityUser>(context);
            }
            catch(Exception e)
            {
                logger.Log(LogLevel.Info, $"exception: {e}");
            }
            if(userStore == null) { return InternalServerError(); }

            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Users.FirstOrDefault(u => u.UserName == account.Username);

            if (!userManager.CheckPassword(user, account.Password))
            {
                logger.Log(LogLevel.Info, "password unauthorized");
                return Unauthorized();
            }

            var authManager = Request.GetOwinContext().Authentication;
            var claimsIdentity = userManager.CreateIdentity(user, WebApiConfig.AuthenticationType);

            authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);

            return Ok();
        }

        [HttpPut]
        [Route("~/api/Accounts/Edit")]
        public async Task<IHttpActionResult> Edit([FromBody]AccountEdit account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state invalid");
            }

            try
            {
                var userStore = new UserStore<IdentityUser>(new AccountDbContext());
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = userManager.Users.First(x => x.UserName == account.Username);
                if(user==null) { throw new ArgumentException("account");  }

                if (!userManager.CheckPassword(user, account.Password))
                {
                    return Unauthorized();
                }

                if(userManager.HasPassword(user.Id))
                {
                    userManager.RemovePassword(user.Id);
                }

                var hashedPw = userManager.PasswordHasher.HashPassword(account.newPassword);

                await userStore.SetPasswordHashAsync(user, hashedPw);
                await userManager.UpdateAsync(user);
                user.UserName = account.newUsername ?? account.Username;
                user.Email = account.newEmail;
                userStore.Context.SaveChanges();

                // Refresh
                Request.GetOwinContext().Authentication.SignOut(WebApiConfig.AuthenticationType);
                var authManager = Request.GetOwinContext().Authentication;
                var claimsIdentity = userManager.CreateIdentity(user, WebApiConfig.AuthenticationType);
                authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);

                return Ok();
            }
            catch
            {
                return BadRequest("Invalid user");
            }
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
