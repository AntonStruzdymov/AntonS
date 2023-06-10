using AntonDB;
using AntonS.DB.CQS.Commands;
using AntonS.DB.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.CQS.CommandsHandler
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand>
    {
        private readonly AntonDBContext _context;
        public AddRefreshTokenCommandHandler(AntonDBContext context)
        {
            _context = context;
        }
        public async Task Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(new RefreshToken()
            {
                UserId = request.UserId,
                Value = request.RefreshToken
            });
            await _context.SaveChangesAsync();
        }
    }
}
