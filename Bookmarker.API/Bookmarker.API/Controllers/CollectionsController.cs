﻿using Bookmarker.API.Models;
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
        [AllowAnonymous]
        public IHttpActionResult Get(string search = null, string sort = null)
        {
            try
            {
                //fetch
                var collections = _collectionRepository.Table;
                if (collections == null) { return Ok(collections); }
                //Search
                var collectionsList = collections.ToList();
                if (search != null)
                {
                    collectionsList = Logic.Library.Search(collectionsList, search);
                }
                //Sort
                if (sort != null)
                {
                    Logic.Library.Sort(ref collectionsList, sort);
                }
                //convert
                var apiColls = new List<CollectionAPI>();
                foreach (var c in collectionsList)
                {
                    if(!c.Private)
                    {
                        apiColls.Add(new CollectionAPI(c));
                    }
                }
                return Ok(apiColls);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Collections/5
        [AllowAnonymous]
        public IHttpActionResult Get(Guid id)
        {
            try
            {
                var collection = _collectionRepository.GetById(id);
                //TODO Return some HTTP request if collection is set to private!
                return collection != null ? Ok(new CollectionAPI(collection)) : (IHttpActionResult) NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST: api/Collections
        public IHttpActionResult Post([FromBody]CollectionAPI collectionApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _collectionRepository.Insert(collectionApi.ToCollectionNoBookmarks());
            return Ok();
        }

        // PUT: api/Collections
        public IHttpActionResult Put([FromBody]CollectionAPI collectionApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                Collection oldcollection = _collectionRepository.GetById(collectionApi.Id);
                if(oldcollection == null)
                {
                    _collectionRepository.Insert(collectionApi.ToCollectionNoBookmarks());
                }
                else
                {
                    Collection updatedColl = collectionApi.ToCollectionNoBookmarks();
                    updatedColl.Bookmarks = oldcollection.Bookmarks;
                    updatedColl.Created = oldcollection.Created;
                    _collectionRepository.Update(updatedColl);
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
                Collection collection = _collectionRepository.GetById(id);
                _collectionRepository.Delete(collection);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
