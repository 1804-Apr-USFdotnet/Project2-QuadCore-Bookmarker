using Bookmarker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bookmarker.API.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public async Task<ActionResult> Users()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:55287/api/Users");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var users = await response.Content.ReadAsAsync<IEnumerable<User>>();
            return View(users);
        }
    }
}
