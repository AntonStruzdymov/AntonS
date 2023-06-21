using AntonS.Core.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.CQS.Queries
{
    public class GetUserByRefreshTokenQuery : IRequest<UserDTO>
    {
        public Guid RefreshToken { get; set; }
    }
}
