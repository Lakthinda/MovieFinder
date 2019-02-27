using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MovieFinder.Client.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace MovieFinder.Client.Test
{
    public class FilmWorldClientTests
    {
        private Mock<IConfiguration> configuration;
        private readonly Mock<ILogger<FilmWorldClient>> filmWorldClientlogger = new Mock<ILogger<FilmWorldClient>>();

        [Fact]
        public void Constructor_With_EmptyAPIURL()
        {
            //Arrange
            configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c["MovieFinder.APIURL"])
                         .Returns("");
            Mock<HttpClient> client = new Mock<HttpClient>();


            //Act
            Action action = () => new FilmWorldClient(configuration.Object,client.Object,filmWorldClientlogger.Object);
            
            //Assert
            var exception = Assert.Throws<KeyNotFoundException>(action);
            Assert.Contains("APIURL", exception.Message);
        }

        [Fact]
        public void Constructor_With_Incorrect_URL()
        {
            //Arrange
            configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c["MovieFinder.APIURL"])
                         .Returns("Not a URL");
            Mock<HttpClient> client = new Mock<HttpClient>();


            //Act
            Action action = () => new FilmWorldClient(configuration.Object, client.Object, filmWorldClientlogger.Object);

            //Assert
            var exception = Assert.Throws<KeyNotFoundException>(action);
            Assert.Contains("APIURL", exception.Message);
        }

        [Fact]
        public void Constructor_With_EmptyAPIKey()
        {
            //Arrange
            configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c["MovieFinder.APIURL"])
                         .Returns("http://testURL.com");
            configuration.SetupGet(c => c["MovieFinder.APIKey"])
                         .Returns("");
            Mock<HttpClient> client = new Mock<HttpClient>();


            //Act
            Action action = () => new FilmWorldClient(configuration.Object, client.Object, filmWorldClientlogger.Object);

            //Assert
            var exception = Assert.Throws<KeyNotFoundException>(action);
            Assert.Contains("APIKey", exception.Message);
        }

        [Fact]
        public void Constructor_With_APIURL_AND_APIKey()
        {
            //Arrange
            configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c["MovieFinder.APIURL"])
                         .Returns("http://testURL.com");
            configuration.SetupGet(c => c["MovieFinder.APIKey"])
                         .Returns("API KEY Here");
            Mock<HttpClient> client = new Mock<HttpClient>();


            //Act
            IMovieAPIClient<FilmWorldClient> sut = new FilmWorldClient(configuration.Object, client.Object, filmWorldClientlogger.Object);

            //Assert
            Assert.NotNull(sut);
        }
    }
}
