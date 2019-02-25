using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieFinder.Client.Data
{
    public interface IMovieAPIClient <T>
    {
        Task<IEnumerable<Movie>> GetMovieList(CancellationToken cancellationToken);
        Task<MovieDetail> GetMovieDetails(CancellationToken cancellationToken, string ID);
    }
}
