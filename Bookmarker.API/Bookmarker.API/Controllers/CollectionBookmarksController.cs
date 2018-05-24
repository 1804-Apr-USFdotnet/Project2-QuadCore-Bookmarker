using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bookmarker.Models;
using Bookmarker.Repositories;

using Bookmarker.API.Models;

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

        [AllowAnonymous]
        [Route("api/collections/{collectionId}/bookmarks")]
        public IHttpActionResult Get(Guid collectionId, string search = null, string sort = null)
        {
            try
            {
                var collection = _repo.GetById(collectionId);
                if (collection == null)
                {
                    return BadRequest($"No collection exists with ID ${collectionId}");
                }

                if (collection.Bookmarks != null)
                {
                    //Search
                    var bookmarksList = collection.Bookmarks.ToList();
                    if (search != null)
                    {
                        bookmarksList = Logic.Library.Search(bookmarksList, search);
                    }
                    //Sort
                    if (sort != null)
                    {
                        Logic.Library.Sort(ref bookmarksList, sort);
                    }

                    List<BookmarkAPI> apiList = new List<BookmarkAPI>();
                    foreach(var bookmark in bookmarksList)
                    {
                        apiList.Add(new BookmarkAPI(bookmark));
                    }
                    return Ok(apiList);
                }
                else
                {
                    List<BookmarkAPI> apiList = new List<BookmarkAPI>();

                    Repository<Bookmark> repo = new Repository<Bookmark>(new BookmarkerContext());
                    var bookmarks = repo.Table.Where(x => x.CollectionId == collectionId);
                    foreach (var b in bookmarks)
                    {
                        apiList.Add(new BookmarkAPI(b));
                    }
                    return Ok(apiList);
                }
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }

        [Route("api/collections/{collectionId}/bookmarks/{index:int}")]
        public IHttpActionResult GetByIndex(Guid collectionId, int index)
        {
            try
            {
                var collection = _repo.GetById(collectionId);
                if (collection == null || index <= 0)
                {
                    return BadRequest($"No collection exists with ID ${collectionId}");
                }

                if (collection.Bookmarks == null || collection.Bookmarks.Count() < index)
                {
                    return NotFound();
                }

                var bookmarkApi = new BookmarkAPI(collection.Bookmarks.ElementAt(index - 1));
                return Ok(bookmarkApi);
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }

        [Route("api/collections/{collectionId}/bookmarks/{id}")]
        public IHttpActionResult GetById(Guid collectionId, Guid id)
        {
            try
            {
                var collection = _repo.GetById(collectionId);
                if (collection == null)
                {
                    return BadRequest($"No collection exists with ID ${collectionId}");
                }

                if (collection.Bookmarks == null || !collection.Bookmarks.Any(x => x.Id == id))
                {
                    return NotFound();
                }

                var bookmarkApi = new BookmarkAPI(collection.Bookmarks.FirstOrDefault(x => x.Id == id));
                return Ok(bookmarkApi);
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return InternalServerError(ex);
            }
        }

    }
}
