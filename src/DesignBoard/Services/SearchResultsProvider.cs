using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DesignBoard.Model;
using Microsoft.Framework.Caching.Memory;
using Newtonsoft.Json;

namespace DesignBoard.Services
{
    public class SearchResultsProvider : ISearchResultsProvider
    {
        const string QUERY = "https://api.github.com/search/issues?per_page=100&sort=created&q=is:issue user:aspnet is:open -repo:aspnet/routing -repo:aspnet/announcements -repo:aspnet/aspnet-docker -repo:aspnet/templates -repo:aspnet/musicstore -repo:aspnet/tooling -repo:aspnet/tooling-internal -repo:aspnet/external -repo:aspnet/mvc -repo:aspnet/razor -repo:aspnet/entityframework -repo:aspnet/identity -repo:aspnet/docs -repo:aspnet/home label:\"needs design\"";

        private readonly HttpClient _client = new HttpClient();

        public async Task<SearchResults> GetSearchResultsAsync()
        {
            var request = BuildHttpRequest(QUERY);
            var response = await _client.SendAsync(request);
            // TODO: Handle gracefully
            response.EnsureSuccessStatusCode();
            var dataStream = await response.Content.ReadAsStreamAsync();

            SearchResults results = null;
            using (var reader = new StreamReader(dataStream))
            {
                var serializer = new JsonSerializer();
                results = serializer.Deserialize<SearchResults>(new JsonTextReader(reader));
            }

            // HACK: Parse the issue url to get the repository name
            var repos = new Dictionary<string, Repository>();

            foreach (var item in results.items)
            {
                var prefixLength = "https://github.com/aspnet/".Length;
                var issueUrl = item.html_url;
                var repoUrl = issueUrl.Substring(0, issueUrl.IndexOf('/', prefixLength + 1));
                var repoName = repoUrl.Substring(prefixLength);

                Repository repo;
                if (!repos.TryGetValue(repoName, out repo))
                {
                    repo = new Repository();
                    repos[repoName] = repo;
                }

                repo.name = repoName;
                repo.html_url = repoUrl;
                item.repo = repo;
            }
            return results;
        }

        public HttpRequestMessage BuildHttpRequest(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Add("User-Agent", "DesignBoard"); //Github returns a malformed response if there is no user-agent.
            return requestMessage;
        }
    }
}
