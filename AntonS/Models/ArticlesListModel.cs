namespace AntonS.Models
{
    public class ArticlesListModel
    {               
        public List<ArticleShortModel> Articles { get; set; }
        public PageInfo pageInfo { get; set; }
        public string Searched { get; set; }
        public string SortBy { get; set; }
        public int Id { get; set; }
    }
}
