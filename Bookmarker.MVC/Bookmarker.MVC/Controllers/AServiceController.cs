﻿using Bookmarker.MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bookmarker.MVC.Controllers
{
    public abstract class AServiceController : Controller
    {
        protected static readonly HttpClient HttpClient = 
            new HttpClient(new HttpClientHandler() { UseCookies = false });
        private static readonly Uri serviceUri = 
            new Uri(ConfigurationManager.AppSettings.Get("ServiceUri"));
        private static readonly string cookieName = "BookmarkerCookie";

        protected HttpRequestMessage CreateRequestToService(HttpMethod method, string uri)
        {
            var apiRequest = new HttpRequestMessage(method, new Uri(serviceUri, uri));

            string cookieValue = Request.Cookies[cookieName]?.Value ?? ""; // ?. operator new in C# 7

            apiRequest.Headers.Add("Cookie", new CookieHeaderValue(cookieName, cookieValue).ToString());

            return apiRequest;
        }

        public async Task<UserAPI> WhoAmI()
        {
            try
            {
                HttpRequestMessage apiRequest = CreateRequestToService(HttpMethod.Get, "Accounts/WhoAmI");
                HttpResponseMessage apiResponse;
                apiResponse = await HttpClient.SendAsync(apiRequest);
                PassCookiesToClient(apiResponse);
                var user = await apiResponse.Content.ReadAsAsync<UserAPI>();
                return user;
            }
            catch
            {
                return null;
            }
        }

        protected bool PassCookiesToClient(HttpResponseMessage apiResponse)
        {
            if (apiResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                foreach (string value in values)
                {
                    Response.Headers.Add("Set-Cookie", value);
                }
                return true;
            }
            return false;
        }
    }
}