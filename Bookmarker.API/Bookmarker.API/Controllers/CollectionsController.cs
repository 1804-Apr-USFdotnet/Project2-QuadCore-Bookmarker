using Bookmarker.Models;
using Bookmarker.Repositories;
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
    public class CollectionsController : ApiController
    {
        private readonly IDbContext _context;
        private readonly Repository<Collection> _collectionRepository;

        public CollectionsController()
        {
            _context = new BookmarkerContext();
            _collectionRepository = new Repository<Collection>(_context);
        }

        public CollectionsController(IDbContext context)
        {
            _context = context; 
            _collectionRepository = new Repository<Collection>(_context);
        }

        // GET: api/Collections
        public IHttpActionResult Get()
        {
            try
            {
                var collections = _collectionRepository.Table;
                var collection = collections.ToList();
                return collections != null ? Ok(collection) : throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Collections/5
        public IHttpActionResult Get(Guid id)
        {
            return null;
        }

        // POST: api/Collections
        public IHttpActionResult Post([FromBody]Collection collection)
        {
            return null;
        }

        // PUT: api/Collections
        public IHttpActionResult Put([FromBody]Collection collection)
        {
            return null;
        }

        // DELETE: api/Users/5
        public IHttpActionResult Delete(Guid id)
        {
            return null;
        }
    }
}
