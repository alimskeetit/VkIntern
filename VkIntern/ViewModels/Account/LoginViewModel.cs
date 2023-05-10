using System.ComponentModel.DataAnnotations;

namespace VkIntern.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
