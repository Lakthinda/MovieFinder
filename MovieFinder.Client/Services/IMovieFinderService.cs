using MovieFinder.Client.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Client.Services
{
    /// <summary>
    /// Interfance for MovieFinderService
    /// </summary>
    public interface IMovieFinderService
    {
        Task<IEnumerable<Movie>> GetDistinctMovieList();
        Task<MovieDetail> GetCheapestMovieDetailsByTitle(string movieTitle = "");
    }
}
