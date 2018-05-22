using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Bookmarker.API.Startup))]

namespace Bookmarker.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = WebApiConfig.AuthenticationType,
                CookieName = WebApiConfig.CookieName
            });
        }
    }
}
