using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Core.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string CommentText { get; set; }
        public int ArticleID { get; set; }
        
    }
}
