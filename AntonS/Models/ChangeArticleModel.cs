namespace AntonS.Models
{
    public class ChangeArticleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullText { get; set; }
        public double PositivityRating { get; set; }
        public int SourceID { get; set; }
        public bool DeleteArticle { get; set; }
    }
}
