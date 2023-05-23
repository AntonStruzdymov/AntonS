using AntonS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonDB.Entities
{
    public class Source : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SourceURl { get; set; }
        public string SourceRssURL { get; set; }
        public List<Article> Articles { get; set; }
    }
}
