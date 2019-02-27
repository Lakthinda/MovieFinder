using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MovieFinder.Client.Data;

namespace MovieFinder.Client.Services
{
    /// <summary>
    /// MovieFinderService use both <see cref="CinemaWorldClient"/> and <see cref="FilmWorldClient"/> services
    /// </summary>
    public class MovieFinderService : IMovieFinderService
    {
        private readonly IMovieAPIClient<CinemaWorldClient> cinemaWorldClient;
        private readonly IMovieAPIClient<FilmWorldClient> filmWorldClient;
        private readonly ILogger<MovieFinderService> logger;
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Constructor - use both <see cref="CinemaWorldClient"/> and <see cref="FilmWorldClient"/> services
        /// </summary>
        /// <param name="cinemaWorldClient"></param>
        /// <param name="filmWorldClient"></param>
        public MovieFinderService(IMovieAPIClient<CinemaWorldClient> cinemaWorldClient, 
                                  IMovieAPIClient<FilmWorldClient> filmWorldClient,
                                  ILogger<MovieFinderService> logger)
        {
            this.cinemaWorldClient = cinemaWorldClient;
            this.filmWorldClient = filmWorldClient;
            this.logger = logger;
        }
        /// <summary>
        /// Returns list of movies ordered by Year
        /// </summary>
        /// <returns> x</returns>
        public async Task<IEnumerable<Movie>> GetDistinctMovieList()
        {
            var combinedMovieList = await GetCombinedMovieList();
            var distinctMovieList = combinedMovieList.GroupBy(m => m.Title).Select(x => x.First()).OrderByDescending(o => o.Year);

            return distinctMovieList;
        }

        /// <summary>
        /// Returns MovieDetail object
        /// </summary>
        /// <param name="movieTitle"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns a list of movies from both APIs ignoring any errors
        /// </summary>
        /// <returns></returns>
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
                logger.LogError("CinemaWorldClient.GetMovieList()- Exception occured", e);

            }

            try
            {
                filmWorldMovies = await filmWorldClient.GetMovieList(cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                logger.LogError("FilmWorldClient.GetMovieList()- Exception occured", e);
            }

            var completeMovieList = (filmWorldMovies ?? Enumerable.Empty<Movie>()).Concat(cinemaWorldMovies ?? Enumerable.Empty<Movie>());
            return completeMovieList;
        }
        /// <summary>
        /// Search Movies by ID and Returns movie object ignoring any errors
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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
                    logger.LogError("CinemaWorldClient.GetMovieDetails()- Exception occured", e);
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
                    logger.LogError("FilmWorldClient.GetMovieDetails()- Exception occured", e);
                }
            }

            return movieDetail;
        }

    }
}
