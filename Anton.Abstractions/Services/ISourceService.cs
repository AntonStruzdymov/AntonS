using AntonS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Abstractions.Services
{
    public interface ISourceService
    {
        Task<List<SourceDTO>> GetSourcesAsync();
        Task<SourceDTO?> GetSourceByIdAsync(int id);
        Task AddSourceAsync(SourceDTO dto);
    }
}
