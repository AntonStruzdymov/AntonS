namespace AntonS.Models
{
    public class ChangeCommentModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string CommentText { get; set; }
        public int ArticleID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
