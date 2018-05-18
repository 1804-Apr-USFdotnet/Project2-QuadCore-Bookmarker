﻿using Bookmarker.Models;
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
        private static readonly IDbContext _context = new BookmarkerContext();
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
                return users != null ? Ok(users) : throw new NullReferenceException();
            }
            catch
            {
                return InternalServerError();
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
