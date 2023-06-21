using AntonDB;
using AntonS.DB.CQS.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.CQS.QueriesHandler
{
    public class GetUserRoleByUserNameQueryHandler : IRequestHandler<GetUserRoleByUserNameQuery, string>
    {
        private readonly AntonDBContext _context;
        public GetUserRoleByUserNameQueryHandler(AntonDBContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetUserRoleByUserNameQuery request, CancellationToken cancellationToken)
        {
            var role = (await _context.Users.AsNoTracking()
                .Include(user => user.AccessLevel)
                .FirstOrDefaultAsync(user => user.Id.Equals(request.UserId)))
                .AccessLevel.name;
            if(role != null)
            {
                return role;
            }
            else
            {
                throw new ArgumentException("Incorrect user id", nameof(request.UserId));
            }
        }
    }
}
