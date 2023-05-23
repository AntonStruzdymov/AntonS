using AntonS.Core;

namespace AntonDB.Entities
{
    public class Comment : IBaseEntity
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string CommentText { get; set; }
        public int ArticleID { get; set; }
        public Article Article { get; set; }
    }
}
