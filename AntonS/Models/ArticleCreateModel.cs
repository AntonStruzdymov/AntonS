using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AntonS.Models
{
    public class ArticleCreateModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FullText { get; set; }
        [Required]
        public int SourceID { get; set; }

        public List<SelectListItem>? AvailableSources { get; set; }
    }
}
