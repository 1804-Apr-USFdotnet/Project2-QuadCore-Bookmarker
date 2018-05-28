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

        public BookmarksController(IDbContext context)
        {
            _context = context;
            _bookmarkRepository = new Repository<Bookmark>(_context);
        }

        // GET:api/Bookmarks
        [AllowAnonymous]
        public IHttpActionResult Get(string search = null, string sort = null)
        {
            try
            {
                //fetch
                var bookmarks = _bookmarkRepository.Table;
                var apiBookmarks = new List<BookmarkAPI>();
                //Search
                var bookmarksList = bookmarks.ToList();
                if (search != null)
                {
                    bookmarksList = Logic.Library.Search(bookmarksList, search);
                }
                //Sort
                if (sort != null)
                {
                    Logic.Library.Sort(ref bookmarksList, sort);
                }
                //convert
                foreach (var b in bookmarksList)
                {
                    apiBookmarks.Add(new BookmarkAPI(b));
                }
                return bookmarks != null ? Ok(apiBookmarks) : throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET:api/Bookmarks/5
        [AllowAnonymous]
        public IHttpActionResult Get(Guid id)
        {
            try
            {
                var bookmark = _bookmarkRepository.GetById(id);
                return bookmark != null ? Ok(bookmark) : (IHttpActionResult)NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST:api/Bookmarks
        public IHttpActionResult Post([FromBody]BookmarkAPI bookmarkApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _bookmarkRepository.Insert(bookmarkApi.ToBookmarkNoCollection());
            return Ok();
        }

        // PUT:api/Bookmarks
        public IHttpActionResult Put([FromBody]BookmarkAPI bookmarkApi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                Bookmark oldBookmark = _bookmarkRepository.GetById(bookmarkApi.Id);
                if (oldBookmark == null)
                {
                    _bookmarkRepository.Insert(bookmarkApi.ToBookmarkNoCollection());
                }
                else
                {
                    Bookmark updatedBookmark = bookmarkApi.ToBookmarkNoCollection();
                    updatedBookmark.Collection = oldBookmark.Collection;
                    _bookmarkRepository.Update(updatedBookmark);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }

        // DELETE:api/Bookmarks
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                Bookmark bookmark = _bookmarkRepository.GetById(id);
                _bookmarkRepository.Delete(bookmark);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
