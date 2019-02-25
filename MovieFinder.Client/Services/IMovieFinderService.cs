using MovieFinder.Client.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Client.Services
{
    public interface IMovieFinderService
    {
        Task<IEnumerable<Movie>> GetDistinctMovieList();
        Task<MovieDetail> GetCheapestMovieDetailsByTitle(string movieTitle = "");

        //Task<IEnumerable<Movie>> GetMovieList(HttpClient client);
    }
}
