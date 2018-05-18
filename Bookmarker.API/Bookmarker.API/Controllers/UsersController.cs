using Bookmarker.Models;
using Bookmarker.Repositories;
using Bookmarker.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Bookmarker.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        private static readonly IDbContext _context = new BookmarkerTestContext();
        private static readonly Repository<User> _userRepository = new Repository<User>(_context);

        public UsersController()
        {
        }

        // GET: api/Users
        public IHttpActionResult Get()
        {
            try
            {
                var users = _userRepository.Table;

                if (users != null)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: api/Users/5
        public User Get(Guid id)
        {
            return _userRepository.GetById(id);
        }

        // POST: api/Users
        public void Post([FromBody]User user)
        {
            _userRepository.Insert(user);
        }

        // PUT: api/Users/5
        public void Put([FromBody]User user)
        {
            _userRepository.Update(user);
        }

        // DELETE: api/Users/5
        public void Delete(Guid id)
        {
            User user = _userRepository.GetById(id);
            _userRepository.Delete(user);
        }
    }
}
