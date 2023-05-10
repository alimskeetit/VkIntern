using System.ComponentModel.DataAnnotations;

namespace VkIntern.ViewModels.UserState
{
    public class UserStateViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
