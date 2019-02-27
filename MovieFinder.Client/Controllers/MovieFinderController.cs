using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Client.Models;
using MovieFinder.Client.Services;

namespace MovieFinder.Client.Controllers
{
    /// <summary>
    /// Provide JSON results for async calls
    /// Inject <see cref="IMovieFinderService"/>
    /// </summary>
    [Route("api/moviefinder")]
    public class MovieFinderController : Controller
    {
        private readonly IMovieFinderService movieFinderService;
        public MovieFinderController(IMovieFinderService movieFinderService)
        {
            this.movieFinderService = movieFinderService;            
        }

        /// <summary>
        /// Retuns distinct movie list from both apis
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetMovies()
        {
            var movieList = await movieFinderService.GetDistinctMovieList();

            var result = Mapper.Map<IEnumerable<MovieDto>>(movieList);
            return Ok(result);
        }
        /// <summary>
        /// Returns a movie based on movie title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("{title}")]
        public async Task<IActionResult> GetMovie(string title)
        {
            var movie = await movieFinderService.GetCheapestMovieDetailsByTitle(title);
            if(movie == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<MovieDetailDto>(movie);
            return Ok(result);
        }

    }
}
