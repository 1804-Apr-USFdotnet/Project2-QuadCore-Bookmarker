using Bookmarker.API.Models;
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
        public IHttpActionResult Get(string sort = null)
        {
            try
            {
                //Fetch
                var users = _userRepository.Table;
                if(users == null) { return Ok(users); }
                //Sort
                var usersList = users.ToList();
                Logic.Library.Sort(ref usersList, sort);
                //Convert
                var apiUsers = new List<UserAPI>();
                foreach (var u in usersList)
                {
                    apiUsers.Add(new UserAPI(u));
                }
                return Ok(apiUsers);
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
                return user != null ? Ok(new UserAPI(user)) : (IHttpActionResult) NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST: api/Users
        public IHttpActionResult Post([FromBody]UserAPI userApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _userRepository.Insert(userApi.ToUserNoCollections());
            return Ok();
        }

        // PUT: api/Users
        public IHttpActionResult Put([FromBody]UserAPI userApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                User oldUser = _userRepository.GetById(userApi.Id);
                if(oldUser == null)
                {
                    _userRepository.Insert(userApi.ToUserNoCollections());
                }
                else
                {
                    User updatedUser = userApi.ToUserNoCollections();
                    updatedUser.Collections = oldUser.Collections;
                    updatedUser.Created = oldUser.Created;
                    _userRepository.Update(updatedUser);
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
