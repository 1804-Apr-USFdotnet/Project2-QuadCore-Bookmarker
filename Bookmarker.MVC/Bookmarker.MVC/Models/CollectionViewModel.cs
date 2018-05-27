using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class CollectionViewModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int Rating { get; set; }
        public bool Private { get; set; }
        public Guid OwnerId { get; set; }


        public IEnumerable<BookmarkViewModel> Bookmarks;

        public async Task<bool> InitBookmarksAsync()
        {
            Uri serviceUri = new Uri(ConfigurationManager.AppSettings.Get("ServiceUri"));
            HttpRequestMessage apiRequest 
                = new HttpRequestMessage(HttpMethod.Get, new Uri(serviceUri, $"collections/{Id}/bookmarks"));
            HttpResponseMessage apiResponse;
            try
            {
                HttpClient HttpClient = 
                    new HttpClient(new HttpClientHandler() { UseCookies = false });
                apiResponse = await HttpClient.SendAsync(apiRequest);
                Bookmarks = await apiResponse.Content.ReadAsAsync<IEnumerable<BookmarkViewModel>>();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}