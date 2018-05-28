using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bookmarker.MVC.Models;
using NLog;

namespace Bookmarker.MVC.Controllers
{
    public class HomeController : AServiceController
    {
        public async Task<ActionResult> GuestLanding()
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

            PassCookiesToClient(apiResponse);
            return RedirectToAction("TopCollections", "Collections");
        }

        public async Task<ActionResult> Home()
        {
            try
            {
                var user = await WhoAmI();
                if(user==null)
                {
                    return RedirectToAction("GuestLanding", "Home");
                }
                return RedirectToAction("MyCollections", "Collections");
            }
            catch
            {
                return RedirectToAction("GuestLanding", "Home");
            }
        }

        public async Task<ActionResult> UserProfile(Guid id, string search, string sort = "name")
        {
            var me = await WhoAmI();
            if(me != null && me.Id == id) { return RedirectToAction("MyCollections", "Collections");  }

            // Get user
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, $"Users/{id}");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            PassCookiesToClient(apiResponse);
            var user = await apiResponse.Content.ReadAsAsync<AccountViewModel>();


            // Now get user's collections
            apiRequest = CreateRequestToService(HttpMethod.Get, $"users/{id}/collections?search=" + search + "&sort=" + sort);

            apiResponse = null;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            PassCookiesToClient(apiResponse);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode != HttpStatusCode.Unauthorized)
                {
                    return View("Error");
                }
                return RedirectToAction("UserDetails", "Accounts");
            }
            else
            {
                //await user.InitCollectionsAsync();
                user.Collections = await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>();
                user.Collections = user.Collections.Where(x => x.Private == false);

                foreach (var collection in user.Collections)
                {
                    await collection.InitBookmarksAsync();
                }
                return View(user);
            }
        }

        public async Task<ActionResult> UsersList()
        {
            HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Users");

            HttpResponseMessage apiResponse;
            try
            {
                apiResponse = await HttpClient.SendAsync(apiRequest);
            }
            catch
            {
                return View("Error");
            }

            PassCookiesToClient(apiResponse);

            if (!apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.StatusCode != HttpStatusCode.Unauthorized)
                {
                    return View("Error");
                }
                TempData["Message"] = "Please log in.";
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                return View(await apiResponse.Content.ReadAsAsync<IEnumerable<AccountViewModel>>());
            }
        }
    }
}