using System;
using System.ComponentModel.DataAnnotations;

namespace BookStoreDomain.Models
{
    public class UserRoleCreateViewModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        public int RoleId { get; set; }
    }
}
