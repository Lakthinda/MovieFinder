using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MovieFinder.Client.Data;
using MovieFinder.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MovieFinder.Client.Test
{
    public class MovieFinderServiceTests
    {
        private IMovieFinderService sut;
        private Mock<IConfiguration> configuration;
        
        public MovieFinderServiceTests()
        {
            configuration = new Mock<IConfiguration>();
            configuration.SetupGet(c => c["MovieFinder.APIURL"])
                        .Returns("http://testURL.com");
            configuration.SetupGet(c => c["MovieFinder.APIKey"])
                         .Returns("API KEY Here");
        }

        #region GetDistinctMovieList_Tests
        [Fact]
        public async void GetDistinctMovieList_Must_Ignore_Exception_In_CinemaWorldAPI_And_Return_Results_In_FilmWorldAPI()
        {
            //Arrange
            // Create Error client
            var unAuthorisedResponseHttpMessageHandler = new Mock<HttpMessageHandler>();
            unAuthorisedResponseHttpMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Forbidden
                    });
            var httpClient = new HttpClient(unAuthorisedResponseHttpMessageHandler.Object);
            IMovieAPIClient<CinemaWorldClient> cinemaWorldClient = new CinemaWorldClient(configuration.Object, httpClient);
            // Mock filmWorldClient with data
            Mock<IMovieAPIClient<FilmWorldClient>> filmWorldClient = new Mock<IMovieAPIClient<FilmWorldClient>>();
            filmWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieList);

            sut = new MovieFinderService(cinemaWorldClient,filmWorldClient.Object);

            //Act
            var result = await sut.GetDistinctMovieList();
                        
            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result.ToList());
            Assert.Equal(testMovieList.Select(m => m.ID).FirstOrDefault(), result.Select(r => r.ID).FirstOrDefault());
        }

        [Fact]
        public async void GetDistinctMovieList_Must_Ignore_Exception_In_FilmWorldAPI_And_Return_Results_In_CinemaWorldAPI()
        {
            //Arrange
            // Create Error client
            var unAuthorisedResponseHttpMessageHandler = new Mock<HttpMessageHandler>();
            unAuthorisedResponseHttpMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.InternalServerError
                    });
            var httpClient = new HttpClient(unAuthorisedResponseHttpMessageHandler.Object);
            IMovieAPIClient<FilmWorldClient> filmWorldClient = new FilmWorldClient(configuration.Object, httpClient);
            // Mock filmWorldClient with data
            Mock<IMovieAPIClient<CinemaWorldClient>> cinemaWorldClient = new Mock<IMovieAPIClient<CinemaWorldClient>>();
            cinemaWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieList);

            sut = new MovieFinderService(cinemaWorldClient.Object, filmWorldClient);

            //Act
            var result = await sut.GetDistinctMovieList();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result.ToList());
            Assert.Equal(testMovieList.Select(m => m.ID).FirstOrDefault(), result.Select(r => r.ID).FirstOrDefault());
        }

        [Fact]
        public async void GetDistinctMovieList_Return_Extra_Movies_From_CinemaWorldAPI()
        {
            //Arrange
            Mock<IMovieAPIClient<CinemaWorldClient>> cinemaWorldClient = new Mock<IMovieAPIClient<CinemaWorldClient>>();
            cinemaWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieListExtra); // Extra Movies

            // Mock filmWorldClient with data
            Mock<IMovieAPIClient<FilmWorldClient>> filmWorldClient = new Mock<IMovieAPIClient<FilmWorldClient>>();
            filmWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieList);

            sut = new MovieFinderService(cinemaWorldClient.Object, filmWorldClient.Object);

            //Act
            var result = await sut.GetDistinctMovieList();

            //Assert
            Assert.NotEmpty(result);
            Assert.IsType<List<Movie>>(result.ToList());
            Assert.Equal(3, result.Count()); // Should return only 3 records
            Assert.NotEmpty(testMovieList.Where(m => m.Title.Equals("Test Movie",StringComparison.OrdinalIgnoreCase))); // check for common record
            Assert.Single(testMovieList.Where(m => m.Title.Equals("Test Movie", StringComparison.OrdinalIgnoreCase))); // Check for one common record
            Assert.Equal("Test movie two", result.Where(r => r.Title.Equals("Test movie two",StringComparison.OrdinalIgnoreCase)).Select(r => r.Title).FirstOrDefault()); // Check for the first extra record
            Assert.Equal("Test movie three", result.Where(r => r.Title.Equals("Test movie three", StringComparison.OrdinalIgnoreCase)).Select(r => r.Title).FirstOrDefault()); // Check for the second extra record
        }

        [Fact]
        public async void GetDistinctMovieList_Return_Extra_Movies_From_FilmWorldAPI()
        {
            //Arrange
            Mock<IMovieAPIClient<CinemaWorldClient>> cinemaWorldClient = new Mock<IMovieAPIClient<CinemaWorldClient>>();
            cinemaWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieList); 

            // Mock filmWorldClient with data
            Mock<IMovieAPIClient<FilmWorldClient>> filmWorldClient = new Mock<IMovieAPIClient<FilmWorldClient>>();
            filmWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieListExtra);// Extra Movies

            sut = new MovieFinderService(cinemaWorldClient.Object, filmWorldClient.Object);

            //Act
            var result = await sut.GetDistinctMovieList();

            //Assert
            Assert.NotEmpty(result);
            Assert.IsType<List<Movie>>(result.ToList());
            Assert.Equal(3, result.Count()); // Should return only 3 records
            Assert.NotEmpty(testMovieList.Where(m => m.Title.Equals("Test Movie", StringComparison.OrdinalIgnoreCase))); // check for common record
            Assert.Single(testMovieList.Where(m => m.Title.Equals("Test Movie", StringComparison.OrdinalIgnoreCase))); // Check for one common record
            Assert.Equal("Test movie two", result.Where(r => r.Title.Equals("Test movie two", StringComparison.OrdinalIgnoreCase)).Select(r => r.Title).FirstOrDefault()); // Check for the first extra record
            Assert.Equal("Test movie three", result.Where(r => r.Title.Equals("Test movie three", StringComparison.OrdinalIgnoreCase)).Select(r => r.Title).FirstOrDefault()); // Check for the second extra record
        }
        #endregion

        #region GetCheapestMovieDetailsByTitle_Tests
        [Fact]
        public async void GetCheapestMovieDetailsByTitle_Return_Cheapest_Movie()
        {
            //Arrange
            Mock<IMovieAPIClient<CinemaWorldClient>> cinemaWorldClient = new Mock<IMovieAPIClient<CinemaWorldClient>>();
            cinemaWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieListExtra);
            cinemaWorldClient.Setup(c => c.GetMovieDetails(It.IsAny<CancellationToken>(), It.IsAny<string>()))
                           .ReturnsAsync(cinemaWorldMovieDetail);

            // Mock filmWorldClient with data
            Mock<IMovieAPIClient<FilmWorldClient>> filmWorldClient = new Mock<IMovieAPIClient<FilmWorldClient>>();
            filmWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieList);
            filmWorldClient.Setup(c => c.GetMovieDetails(It.IsAny<CancellationToken>(), It.IsAny<string>()))
                           .ReturnsAsync(filmWorldMovieDetail);

            sut = new MovieFinderService(cinemaWorldClient.Object, filmWorldClient.Object);

            //Act
            var result = await sut.GetCheapestMovieDetailsByTitle("Test movie");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<MovieDetail>(result);
            Assert.Equal(cinemaWorldMovieDetail.Title, result.Title);  // CinemaWorldMovieDetail is the cheapest
            Assert.Equal(cinemaWorldMovieDetail.Price, result.Price); // Should return CinemaWorldMovieDetail, Price = 100
        }

        [Fact]
        public async void GetCheapestMovieDetailsByTitle_Must_Ignore_Exception_In_CinemaWorldAPI_And_Return_Results_In_FilmWorldAPI()
        {
            //Arrange
            // Create Error client
            var unAuthorisedResponseHttpMessageHandler = new Mock<HttpMessageHandler>();
            unAuthorisedResponseHttpMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Forbidden
                    });
            var httpClient = new HttpClient(unAuthorisedResponseHttpMessageHandler.Object);
            IMovieAPIClient<CinemaWorldClient> cinemaWorldClient = new CinemaWorldClient(configuration.Object, httpClient);

            // Mock filmWorldClient with data
            Mock<IMovieAPIClient<FilmWorldClient>> filmWorldClient = new Mock<IMovieAPIClient<FilmWorldClient>>();
            filmWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieListExtra);
            filmWorldClient.Setup(c => c.GetMovieDetails(It.IsAny<CancellationToken>(), It.IsAny<string>()))
                           .ReturnsAsync(filmWorldMovieDetail);

            sut = new MovieFinderService(cinemaWorldClient, filmWorldClient.Object);

            //Act
            var result = await sut.GetCheapestMovieDetailsByTitle("Test movie");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<MovieDetail>(result);
            Assert.Equal(filmWorldMovieDetail.Title, result.Title);
            Assert.Equal(filmWorldMovieDetail.Price, result.Price); // Should return FilmWorldMovieDetail, Price = 150
        }

        [Fact]
        public async void GetCheapestMovieDetailsByTitle_Must_Ignore_Exception_In_FilmWorldAPI_And_Return_Results_In_CinemaWorldAPI()
        {
            //Arrange
            // Create Error client
            var unAuthorisedResponseHttpMessageHandler = new Mock<HttpMessageHandler>();
            unAuthorisedResponseHttpMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.InternalServerError
                    });
            var httpClient = new HttpClient(unAuthorisedResponseHttpMessageHandler.Object);
            IMovieAPIClient<FilmWorldClient> filmWorldClient = new FilmWorldClient(configuration.Object, httpClient);
            // Mock filmWorldClient with data
            Mock<IMovieAPIClient<CinemaWorldClient>> cinemaWorldClient = new Mock<IMovieAPIClient<CinemaWorldClient>>();
            cinemaWorldClient.Setup(c => c.GetMovieList(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(testMovieList);
            cinemaWorldClient.Setup(c => c.GetMovieDetails(It.IsAny<CancellationToken>(), It.IsAny<string>()))
                           .ReturnsAsync(cinemaWorldMovieDetail);

            sut = new MovieFinderService(cinemaWorldClient.Object, filmWorldClient);

            //Act
            var result = await sut.GetCheapestMovieDetailsByTitle("Test movie");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<MovieDetail>(result);
            Assert.Equal(cinemaWorldMovieDetail.Title, result.Title);
            Assert.Equal(cinemaWorldMovieDetail.Price, result.Price); // Should return FilmWorldMovieDetail, Price = 100
        }
        #endregion

        #region TestData
        private List<Movie> testMovieList => new List<Movie>()
        {
            new Movie()
            {
                ID = "cw1234",
                Poster = "Test.jpg",
                Title = "Test movie",
                Type = "PG",
                Year = 1900
            }
        };
        private List<Movie> testMovieListExtra => new List<Movie>()
        {
            new Movie()
            {
                ID = "fm4321",
                Poster = "Test.jpg",
                Title = "Test movie",
                Type = "PG",
                Year = 1900
            },
            new Movie()
            {
                ID = "fm5678",
                Poster = "Test2.jpg",
                Title = "Test movie two",
                Type = "PG",
                Year = 1900
            },
            new Movie()
            {
                ID = "fm9876",
                Poster = "Test3.jpg",
                Title = "Test movie three",
                Type = "PG",
                Year = 1900
            }
        };

        private MovieDetail cinemaWorldMovieDetail = new MovieDetail
        {
            ID = "cm1234",
            Title = "Test movie",
            Price = 100
        };

        private MovieDetail filmWorldMovieDetail = new MovieDetail
        {
            ID = "fm4321",
            Title = "Test movie",
            Price = 150
        };
        #endregion

    }
}
