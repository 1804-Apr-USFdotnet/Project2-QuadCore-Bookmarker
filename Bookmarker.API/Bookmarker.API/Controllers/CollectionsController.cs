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
            try
            {
                var collection = _collectionRepository.GetById(id);
                return collection != null ? Ok(collection) : (IHttpActionResult) NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST: api/Collections
        public IHttpActionResult Post([FromBody]Collection collection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _collectionRepository.Insert(collection);
            return Ok();
        }

        // PUT: api/Collections
        public IHttpActionResult Put([FromBody]Collection collection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                Collection oldcollection = _collectionRepository.GetById(collection.Id);
                if(oldcollection == null)
                {
                    _collectionRepository.Insert(collection);
                }
                else
                {
                    _collectionRepository.Update(collection);
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
            return null;
        }
    }
}
