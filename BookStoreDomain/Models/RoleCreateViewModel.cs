using System.ComponentModel.DataAnnotations;

namespace BookStoreDomain.Models
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; }
    }
}
