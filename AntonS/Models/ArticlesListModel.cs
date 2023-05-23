namespace AntonS.Models
{
    public class ArticlesListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullText { get; set; }
        public string Searched { get; set; }
        public List<ArticlesListModel> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
