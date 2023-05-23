using AntonDB.Entities;
using AntonS.Abstractions;
using AntonS.Abstractions.Services;
using AntonS.Core.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.Business
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddSourceAsync(SourceDTO dto)
        {
            await _unitOfWork.Sources.AddAsync(_mapper.Map<Source>(dto));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<SourceDTO?> GetSourceByIdAsync(int id)
        {
            var sourceId = await _unitOfWork.Sources.GetByIdAsync(id);
            return _mapper.Map<SourceDTO>(sourceId);
        }

        public async Task<List<SourceDTO>> GetSourcesAsync()
        {
            var sources = await _unitOfWork.Sources.GetAllAsync();
            return sources.Select(s=> _mapper.Map<SourceDTO>(s)).ToList();
        }
    }
}
