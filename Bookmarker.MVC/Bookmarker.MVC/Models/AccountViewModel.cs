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
    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage =
            "Password must be between 8 and 40 characters inclusive.")]
        public string Password { get; set; }
        public string Email { get; set; }

        public IEnumerable<CollectionViewModel> Collections { get; set; }

        public async Task<bool> InitCollectionsAsync(string search = null, string sort = "name")
        {
            Uri serviceUri = new Uri(ConfigurationManager.AppSettings.Get("ServiceUri"));
            HttpRequestMessage apiRequest 
                = new HttpRequestMessage(HttpMethod.Get, new Uri(serviceUri, $"users/{Id}/collections?search=" + search + "&sort=" + sort));
            HttpResponseMessage apiResponse;
            try
            {
                HttpClient HttpClient = 
                    new HttpClient(new HttpClientHandler() { UseCookies = false });
                apiResponse = await HttpClient.SendAsync(apiRequest);
                Collections = await apiResponse.Content.ReadAsAsync<IEnumerable<CollectionViewModel>>();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}