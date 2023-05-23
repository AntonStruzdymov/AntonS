namespace AntonS.Models
{
    public class ArticleWithCommentModel
    {
        public ArticlePreviewModel _articlePreview { get; set; }
        public CommentModel _comment { get; set; }
        public List<CommentModel> _comments { get; set; }
    }
}
