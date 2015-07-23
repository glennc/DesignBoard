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
using DesignBoard.Services;

namespace DesignBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchResultsProvider _searchResultsProvider;

        public HomeController(ISearchResultsProvider searchResultsProvider)
        {
            _searchResultsProvider = searchResultsProvider;
        }

        public async Task<IActionResult> Index()
        {
            SearchResults results = await _searchResultsProvider.GetSearchResultsAsync();
            return View(results);
        }

        [Route("/milestones")]
        public async Task<IActionResult> ByMilestone()
        {
            SearchResults results = await _searchResultsProvider.GetSearchResultsAsync();
            return View("Milestones", results);
        }
    }
}
