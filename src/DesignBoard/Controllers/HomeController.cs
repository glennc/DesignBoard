using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using DesignBoard.Model;

namespace DesignBoard.Controllers
{
    public class HomeController : Controller
    {
        const string QUERY = "https://api.github.com/search/issues?sort=created&q=is:issue user:aspnet is:open -repo:aspnet/routing -repo:aspnet/announcements -repo:aspnet/aspnet-docker -repo:aspnet/templates -repo:aspnet/musicstore -repo:aspnet/tooling -repo:aspnet/tooling-internal -repo:aspnet/external -repo:aspnet/mvc -repo:aspnet/razor -repo:aspnet/entityframework -repo:aspnet/identity -repo:aspnet/docs -repo:aspnet/home label:\"needs design\"";
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var request = BuildHttpRequest(QUERY);
            var response = await httpClient.SendAsync(request);
            var dataStream = await response.Content.ReadAsStreamAsync();

            SearchResults results = null;
            using (var reader = new StreamReader(dataStream))
            {
                JsonSerializer serializer = new JsonSerializer();
                results = serializer.Deserialize<SearchResults>(new JsonTextReader(reader));
            }

            return View(results);
        }

        public HttpRequestMessage BuildHttpRequest(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Add("User-Agent", "DesignBoard"); //Github returns a malformed response if there is no user-agent.
            return requestMessage;
        }
    }
}
