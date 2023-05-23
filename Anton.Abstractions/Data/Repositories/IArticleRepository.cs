using AntonDB.Entities;
using AntonS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Data.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
       Task<List<Article>> GetArticlesByName(string name);
        

    }
}
