using AntonS.Core.DTOs;

namespace AntonS.Models
{
    public class ChoseCommentToEditModel
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public string UserName { get; set; }
        public List<CommentDTO> Comments { get; set; }

    }
}
