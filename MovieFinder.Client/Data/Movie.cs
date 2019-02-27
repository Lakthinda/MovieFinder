namespace MovieFinder.Client.Data
{
    /// <summary>
    /// Movie data object
    /// </summary>
    public class Movie
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}
