using AntonDB;
using AntonDB.Entities;
using AntonS.Abstractions.Data.Repositories;
using AntonS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Repositories.Implementation
{
    public class SourceRepository : Repository<Source>, ISourceRepository
    {
        public SourceRepository(AntonDBContext context) : base (context) { }
    }
}
