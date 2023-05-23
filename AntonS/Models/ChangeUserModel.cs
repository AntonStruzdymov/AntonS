namespace AntonS.Models
{
    public class ChangeUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AcessLevelID { get; set; }
        public bool DeleteUser { get; set; }
        public string ChosenProperty { get; set; }
        public string ChosenPropertyValue { get; set;}
    }
}
