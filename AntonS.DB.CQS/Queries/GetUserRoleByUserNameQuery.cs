using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.CQS.Queries
{
    public class GetUserRoleByUserNameQuery : IRequest<string>
    {
        public int UserId { get; set; }
    }
}
