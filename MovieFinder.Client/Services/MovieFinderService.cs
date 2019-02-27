using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieFinder.Client.Data;

namespace MovieFinder.Client.Services
{
    public class MovieFinderService : IMovieFinderService
    {
        private readonly IMovieAPIClient<CinemaWorldClient> cinemaWorldClient;
        private readonly IMovieAPIClient<FilmWorldClient> filmWorldClient;
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        
        public MovieFinderService(IMovieAPIClient<CinemaWorldClient> cinemaWorldClient, IMovieAPIClient<FilmWorldClient> filmWorldClient)
        {
            this.cinemaWorldClient = cinemaWorldClient;
            this.filmWorldClient = filmWorldClient;
        }

        public async Task<IEnumerable<Movie>> GetDistinctMovieList()
        {
            var combinedMovieList = await GetCombinedMovieList();
            var distinctMovieList = combinedMovieList.GroupBy(m => m.Title).Select(x => x.First());

            return distinctMovieList;
        }

        public async Task<MovieDetail> GetCheapestMovieDetailsByTitle(string movieTitle = "")
        {
            var combinedMovieList = await GetCombinedMovieList();
            var movies = combinedMovieList.Where(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase)).ToList();
            List<MovieDetail> movieDetailList = new List<MovieDetail>();
            foreach (var movie in movies)
            {
                var movieDetail = await GetMovieDetails(movie.ID);
                if (movieDetail != null)
                {
                    movieDetailList.Add(movieDetail);
                }
            }

            var cheapestMovie = movieDetailList.OrderBy(m => m.Price).FirstOrDefault();

            return cheapestMovie;
        }
        private async Task<IEnumerable<Movie>> GetCombinedMovieList()
        {
            IEnumerable<Movie> cinemaWorldMovies = null;
            IEnumerable<Movie> filmWorldMovies = null;
            try
            {
                cinemaWorldMovies = await cinemaWorldClient.GetMovieList(cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                //TODO log error
            }

            try
            {
                filmWorldMovies = await filmWorldClient.GetMovieList(cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                //TODO log error
            }

            var completeMovieList = (filmWorldMovies ?? Enumerable.Empty<Movie>()).Concat(cinemaWorldMovies ?? Enumerable.Empty<Movie>());
            return completeMovieList;
        }

        private async Task<MovieDetail> GetMovieDetails(string ID)
        {
            MovieDetail movieDetail = null;
            if (ID.StartsWith("cw", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    movieDetail = await cinemaWorldClient.GetMovieDetails(cancellationTokenSource.Token, ID);
                    return movieDetail;
                }
                catch (Exception e)
                {
                    //TODO error log
                }
            }
            else
            {
                try
                {
                    movieDetail = await filmWorldClient.GetMovieDetails(cancellationTokenSource.Token, ID);
                    return movieDetail;
                }
                catch (Exception e)
                {
                    //TODO error log
                }
            }

            return movieDetail;
        }

    }
}
