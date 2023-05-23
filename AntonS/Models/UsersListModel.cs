using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AntonS.Models
{
    public class UsersListModel
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AcessLevelID { get; set; }
        public List<SelectListItem> AcessLevels { get; set; }
        public List<UsersListModel> Users { get; set; }        
        public int ChosenUserId { get; set; }
        public bool DeleteUser { get; set; }
    }
}
