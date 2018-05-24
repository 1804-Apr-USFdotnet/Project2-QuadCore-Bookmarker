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

        [AllowAnonymous]
        [Route("api/users/{userid}/collections")]
        public IHttpActionResult Get(Guid userid, string sort = null)
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

                var collectionsList = user.Collections.ToList();
                Logic.Library.Sort(ref collectionsList, sort);
                return Ok(collectionsList);
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }

        [Route("api/users/{userid}/collections/{index:int}")]
        public IHttpActionResult GetCollectionByIndex(Guid userid, int index)
        {
            try
            {
                var user = _repo.GetById(userid);
                if (user == null || index <= 0)
                {
                    return BadRequest();
                }

                if (user.Collections == null || user.Collections.Count < index)
                {
                    return NotFound();
                }

                return Ok(user.Collections.ElementAt(index - 1));
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }

        [Route("api/users/{userid}/collections/{id}")]
        public IHttpActionResult GetCollectionById(Guid userid, Guid id)
        {
            try
            {
                var user = _repo.GetById(userid);
                if (user == null)
                {
                    return BadRequest();
                }

                if (user.Collections == null || !user.Collections.Any(x => x.Id == id))
                {
                    return NotFound();
                }

                return Ok(user.Collections.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }
    }
}
