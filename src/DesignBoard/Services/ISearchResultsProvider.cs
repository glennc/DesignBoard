using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesignBoard.Model;

namespace DesignBoard.Services
{
    public interface ISearchResultsProvider
    {
        Task<SearchResults> GetSearchResultsAsync();
    }
}
