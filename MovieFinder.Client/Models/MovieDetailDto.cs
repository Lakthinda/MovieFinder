using System;

namespace MovieFinder.Client.Models
{
    public class MovieDetailDto
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public string Rated { get; set; }
        public DateTimeOffset Released { get; set; }
        public string RunTime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public int Metascore { get; set; }
        public double Rating { get; set; }
        public string Votes { get; set; }
        public double Price { get; set; }
    }
}
