using Bookmarker.Repositories;
using Bookmarker.Models;
using Bookmarker.API.Models;
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
        public IHttpActionResult Get(Guid userid, string search = null, string sort = null)
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

                // TODO: change sort to use API
                var collectionsList = user.Collections.ToList();
                if (search != null)
                {
                    collectionsList = Logic.Library.Search(collectionsList, search);
                }
                if (sort != null)
                {
                    Logic.Library.Sort(ref collectionsList, sort);
                }
                var collApiList = new List<CollectionAPI>();
                foreach (var collection in collectionsList)
                {
                    collApiList.Add(new CollectionAPI(collection));
                }
                return Ok(collApiList);
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

                var collection = new CollectionAPI(user.Collections.ElementAt(index - 1));
                return Ok(collection);
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

                var collection = new CollectionAPI(user.Collections.FirstOrDefault(x => x.Id == id));
                return Ok(collection);
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }
    }
}
