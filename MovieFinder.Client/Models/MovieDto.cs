namespace MovieFinder.Client.Models
{
    /// <summary>
    /// MovieDto - to pass into UI
    /// </summary>
    public class MovieDto
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}