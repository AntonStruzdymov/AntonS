using AntonDB.Entities;
using AntonS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntonS.DB.Entities
{
    public class AccessLevel : IBaseEntity
    {
        public int Id { get; set; }
        public string name { get; set; }
        public List<User> users { get; set; }
    }
}
