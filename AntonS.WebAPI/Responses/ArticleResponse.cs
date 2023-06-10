namespace AntonS.WebAPI.Responses
{
    public class ArticleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullText { get; set; }
        public double PositivityRating { get; set; }
        public string ArticleSourceURL { get; set; }
        public int SourceID { get; set; }
    }
}
