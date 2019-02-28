using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MovieFinder.Client.IntegrationTest
{
    
    public class MovieFinderTests : IClassFixture<WebApplicationFactory<MovieFinder.Client.Startup>>
    {
        private readonly WebApplicationFactory<MovieFinder.Client.Startup> factory;

        public MovieFinderTests(WebApplicationFactory<MovieFinder.Client.Startup> factory)
        {
            this.factory = factory;

        }


        [Theory]
        [InlineData("api/moviefinder/")]
        //[InlineData("/api/moviefinder/Star Wars: Episode III - Revenge of the Sith", Skip = "ToDO")]
        public async void GetMovies(string url)
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var result = await client.GetAsync(url);

            // Assert
            result.EnsureSuccessStatusCode();

        }
    }
}
