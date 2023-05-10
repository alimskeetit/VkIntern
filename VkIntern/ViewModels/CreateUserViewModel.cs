using System.ComponentModel.DataAnnotations;
using DB;

namespace VkIntern.ViewModels
{
    public class CreateUserViewModel
    {
        //[Required(AllowEmptyStrings = false)]
        public string Login { get; set; } = " ";
        //[Required(AllowEmptyStrings = false)]
        public string Password { get; set; } = " ";
    }
}
