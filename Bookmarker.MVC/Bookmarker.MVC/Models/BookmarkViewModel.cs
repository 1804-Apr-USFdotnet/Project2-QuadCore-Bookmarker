using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Bookmarker.MVC.Models
{
    public class BookmarkViewModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string URL { get; set; }
        public int Rating { get; set; }

        public virtual CollectionViewModel Collection { get; set; }
        public Guid CollectionId { get; set; }

        public async Task<bool> InitCollectionAsync(Guid collectionId)
        {
            CollectionId = collectionId;
            Uri serviceUri = new Uri(ConfigurationManager.AppSettings.Get("ServiceUri"));
            HttpRequestMessage apiRequest 
                = new HttpRequestMessage(HttpMethod.Get, new Uri(serviceUri, $"Collections/{CollectionId}"));
            HttpResponseMessage apiResponse;
            try
            {
                HttpClient HttpClient = 
                    new HttpClient(new HttpClientHandler() { UseCookies = false });
                apiResponse = await HttpClient.SendAsync(apiRequest);
                Collection = await apiResponse.Content.ReadAsAsync<CollectionViewModel>();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}