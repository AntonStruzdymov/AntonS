using AntonDB.Entities;
using AntonS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.Entities
{
    public class RefreshToken : IBaseEntity
    {
        public int Id { get; set; }
        public Guid Value { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
