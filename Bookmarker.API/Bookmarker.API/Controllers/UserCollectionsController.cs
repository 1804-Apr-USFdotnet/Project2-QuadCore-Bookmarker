using Bookmarker.Repositories;
using Bookmarker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bookmarker.API.Controllers
{
    public class UserCollectionsController : ApiController
    {
        private readonly IDbContext _context;
        private readonly Repository<User> _repo;

        public UserCollectionsController()
            : this(new BookmarkerContext())
        {
        }

        public UserCollectionsController(IDbContext context)
        {
            _context = context;
            _repo = new Repository<User>(_context);
        }

        [Route("users/{userid}/collections")]
        public IHttpActionResult Get(Guid userid)
        {
            try
            {
                var user = _repo.GetById(userid);
                if (user == null)
                {
                    return BadRequest();
                }

                if (user.Collections == null)
                {
                    return NotFound();
                }

                return Ok(user.Collections.ToList());
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }

        [Route("users/{userid}/collections/{id:int}")]
        public IHttpActionResult GetCollectionByIndex(Guid userid, int id)
        {
            try
            {
                var user = _repo.GetById(userid);
                if (user == null)
                {
                    return BadRequest();
                }

                if (user.Collections == null || user.Collections.Count < id)
                {
                    return NotFound();
                }

                return Ok(user.Collections.ElementAt(id - 1));
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
            throw new NotImplementedException();
        }

        [Route("users/{userid}/collections/{id}")]
        public IHttpActionResult GetCollectionById(Guid userid, Guid id)
        {
            try
            {
                var user = _repo.GetById(userid);
                if (user == null)
                {
                    return BadRequest();
                }

                if (user.Collections == null || user.Collections.Any(x => x.Id.Equals(id)))
                {
                    return NotFound();
                }

                return Ok(user.Collections.FirstOrDefault(x => x.Id.Equals(id)) );
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }
    }
}
