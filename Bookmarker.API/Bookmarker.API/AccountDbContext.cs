using Microsoft.AspNet.Identity.EntityFramework;

namespace Bookmarker.API
{
    public class AccountDbContext : IdentityDbContext<IdentityUser>
    {
        public AccountDbContext() : base("BookmarkerAccountDb")
        {

        }
    }
}
