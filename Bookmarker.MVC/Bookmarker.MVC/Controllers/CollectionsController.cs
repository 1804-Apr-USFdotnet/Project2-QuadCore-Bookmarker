using Bookmarker.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bookmarker.MVC.Controllers
{
    public class CollectionsController : AServiceController
    {
        // GET: Public Collections
        public async Task<ActionResult> PublicCollections()
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Collections");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            return View(await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>());
        }
    }
}