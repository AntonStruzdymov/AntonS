using AntonDB;
using AntonS.Core.DTOs;
using AntonS.DB.CQS.Queries;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.CQS.QueriesHandler
{
    public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, UserDTO>
    {
        private readonly AntonDBContext _context;
        private readonly IMapper _mapper;
        public GetUserByRefreshTokenQueryHandler(AntonDBContext context, IMapper mapper)
        {
            _context= context;
            _mapper= mapper;
        }
        public async Task<UserDTO> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var user = (await _context.RefreshTokens.AsNoTracking()
                .Include(token=>token.User)
                .FirstOrDefaultAsync(token=> token.Value.Equals(request.RefreshToken))).User;
            return (_mapper.Map<UserDTO>(user));
        }
    }
}
