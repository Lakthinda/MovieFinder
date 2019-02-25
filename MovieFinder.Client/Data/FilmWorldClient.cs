using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MovieFinder.Client.Data
{
    public class FilmWorldClient : IMovieAPIClient<FilmWorldClient>
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient client;
        private readonly string APIKey;
        private readonly string APIURL;
        public FilmWorldClient(IConfiguration configuration,HttpClient client)
        {
            this.configuration = configuration;
            this.client = client;

            APIURL = configuration["MovieFinder.APIURL"];
            if (string.IsNullOrEmpty(APIURL) || !Uri.IsWellFormedUriString(APIURL, UriKind.Absolute))
            {
                throw new KeyNotFoundException("APIURL not found or Not a valid URL.");
            }
            APIKey = configuration["MovieFinder.APIKey"];
            if (string.IsNullOrEmpty(APIKey))
            {
                throw new KeyNotFoundException("APIKey not found.");
            }

            this.client = client;
            this.client.BaseAddress = new Uri(APIURL);
            this.client.Timeout = new TimeSpan(0, 0, 30);
            this.client.DefaultRequestHeaders.Clear();
        }
        public async Task<MovieDetail> GetMovieDetails(CancellationToken cancellationToken, string ID)
        {
            var request = new HttpRequestMessage(
                     HttpMethod.Get,
                     "api/filmworld/movie/" + ID);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("x-access-token", APIKey);

            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();

                var movieDetails = stream.ReadAndDeserializeFromJson<MovieDetail>();

                return movieDetails;
            }
        }

        public async Task<IEnumerable<Movie>> GetMovieList(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    "api/filmworld/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("x-access-token", APIKey);

            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();

                var movieList = stream.ReadAndDeserializeFromJson<MovieList>();

                return movieList.Movies;
            }
        }
    }
}
