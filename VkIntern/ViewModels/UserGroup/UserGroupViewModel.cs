using System.ComponentModel.DataAnnotations;

namespace VkIntern.ViewModels.UserGroup
{
    public class UserGroupViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
