using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Core.DTOs
{
    public class ArticleDTO
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
