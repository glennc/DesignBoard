using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DesignBoard.Model;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.Configuration;
using Newtonsoft.Json;

namespace DesignBoard.Services
{
    public class SearchResultsProvider : ISearchResultsProvider
    {
        private readonly string _searchBaseUrl;
        private readonly string _authToken;
        const string DefaultApiBase = "https://api.github.com/";

        private readonly HttpClient _client = new HttpClient();

        public SearchResultsProvider(IConfiguration config)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(config.Get("GitHub:ApiBase") ?? DefaultApiBase)
            };
            var baseQuery = config.Get("GitHub:BaseQuery");
            _searchBaseUrl = $"/search/issues?per_page=100&sort=created&q=is:issue is:open label:\"needs design\" {baseQuery}";
            _authToken = config.Get("GitHub:AuthToken");
        }

        public async Task<SearchResults> GetSearchResultsAsync()
        {
            SearchResults results = await SearchIssues();

            // HACK: Parse the issue url to get the repository name
            var repos = new Dictionary<string, Repository>();

            var items = new List<Item>();

            foreach (var item in results.items)
            {
                if (string.Equals(item?.milestone?.title, "backlog", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
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
                items.Add(item);
            }

            results.items = items.ToArray();
            return results;
        }

        private async Task<SearchResults> SearchIssues()
        {
            var request = BuildHttpRequest(_searchBaseUrl);
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

            return results;
        }

        public HttpRequestMessage BuildHttpRequest(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Add("User-Agent", "DesignBoard"); //Github returns a malformed response if there is no user-agent.
            if (!string.IsNullOrEmpty(_authToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("token", _authToken);
            }
            return requestMessage;
        }
    }
}
