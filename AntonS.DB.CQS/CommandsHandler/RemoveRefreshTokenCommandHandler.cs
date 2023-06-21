using AntonDB;
using AntonS.DB.CQS.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.CQS.CommandsHandler
{
    public class RemoveRefreshTokenCommandHandler : IRequestHandler<RemoveRefreshTokenCommand>
    {
        private readonly AntonDBContext _context;
        public RemoveRefreshTokenCommandHandler(AntonDBContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var rt = await _context.RefreshTokens.FirstOrDefaultAsync(token => token.Value.Equals(request.RefreshToken),cancellationToken);
            _context.RefreshTokens.Remove(rt);
            await _context.SaveChangesAsync();
        }
    }
}
