using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bookmarker.Models;
using Bookmarker.Repositories;

namespace Bookmarker.API.Controllers
{
    public class CollectionBookmarksController : ApiController
    {
        private readonly IDbContext _context;
        private readonly Repository<Collection> _repo;

        public CollectionBookmarksController()
            : this(new BookmarkerContext())
        {

        }

        public CollectionBookmarksController(IDbContext context)
        {
            _context = context;
            _repo = new Repository<Collection>(_context);
        }

        [Route("collections/{collectionId}/bookmarks")]
        public IHttpActionResult Get(Guid collectionId)
        {
            try
            {
                return InternalServerError();
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }

        [Route("collections/{collectionId}/bookmarks/{index:int}")]
        public IHttpActionResult GetByIndex(Guid collectionId, int index)
        {
            return InternalServerError();
        }

        [Route("collections/{collectionId}/bookmarks/{id}")]
        public IHttpActionResult GetById(Guid collectionId, Guid id)
        {
            return InternalServerError();
        }

    }
}
