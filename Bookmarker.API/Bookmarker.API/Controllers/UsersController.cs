using Bookmarker.Models;
using Bookmarker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace Bookmarker.API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        private readonly IDbContext _context;
        private readonly Repository<User> _userRepository;

        public UsersController()
        {
            _context = new BookmarkerContext();
            _userRepository = new Repository<User>(_context);
        }

        public UsersController(IDbContext context)
        {
            _context = context; 
            _userRepository = new Repository<User>(_context);
        }

        // GET: api/Users
        public IHttpActionResult Get()
        {
            try
            {
                var users = _userRepository.Table;
                var user = users.ToList();
                return users != null ? Ok(user) : throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Users/5
        public IHttpActionResult Get(Guid id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                return user != null ? Ok(user) : (IHttpActionResult) NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST: api/Users
        public IHttpActionResult Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _userRepository.Insert(user);
            return Ok();
        }

        // PUT: api/Users
        public IHttpActionResult Put([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                User oldUser = _userRepository.GetById(user.Id);
                if(oldUser == null)
                {
                    _userRepository.Insert(user);
                }
                else
                {
                    user.Created = oldUser.Created;
                    _userRepository.Update(user);
                }
            }
            catch (Exception ex)
            {
                // TODO: Log errors
                return InternalServerError(ex);
            }
            
            return Ok();
        }

        // DELETE: api/Users/5
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                User user = _userRepository.GetById(id);
                _userRepository.Delete(user);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
