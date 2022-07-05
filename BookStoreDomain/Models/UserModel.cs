using System;

namespace BookStoreDomain.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UserSalt { get; set; }

        public bool IsActive { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string AuthorPseudonym { get; set; }


    }


 

}
