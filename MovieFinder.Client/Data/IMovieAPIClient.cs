using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieFinder.Client.Data
{
    /// <summary>
    /// IMovieAPIClient use in both APIs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMovieAPIClient <T>
    {
        Task<IEnumerable<Movie>> GetMovieList(CancellationToken cancellationToken);
        Task<MovieDetail> GetMovieDetails(CancellationToken cancellationToken, string ID);
    }
}
