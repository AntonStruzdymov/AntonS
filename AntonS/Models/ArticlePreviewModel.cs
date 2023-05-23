using AntonDB.Entities;

namespace AntonS.Models
{
    public class ArticlePreviewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FullText { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
