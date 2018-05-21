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
    [EnableCors("*","*","*")]
    public class BookmarksController : ApiController
    {
        private readonly IDbContext _context;
        private readonly Repository<Bookmark> _bookmarkRepository;

        public BookmarksController()
        {
            _context = new BookmarkerContext();
            _bookmarkRepository = new Repository<Bookmark>(_context);
        }

        // GET:api/Bookmarks
        public IHttpActionResult Get ()
        {
            throw new NotImplementedException();
        }

        // GET:api/Bookmarks/5
        public IHttpActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST:api/Bookmarks
        public IHttpActionResult Post([FromBody]BookmarkAPI bookmarkApi)
        {
            throw new NotImplementedException();
        }

        // PUT:api/Bookmarks
        public IHttpActionResult Put([FromBody]BookmarkAPI bookmarkApi)
        {
            throw new NotImplementedException();
        }

        // DELETE:api/Bookmarks
        public IHttpActionResult Delete([FromBody]BookmarkAPI bookmarkApi)
        {
            throw new NotImplementedException();
        }
    }
}
