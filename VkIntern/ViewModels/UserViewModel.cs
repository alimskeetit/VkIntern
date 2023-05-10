using DB;

namespace VkIntern.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DB.UserGroup Group { get; set; }
        public DB.UserState State { get; set; }
    }
}
