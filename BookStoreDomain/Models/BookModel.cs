using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDomain.Models
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string BookDescription { get; set; }
        public int Author { get; set; }
        public string CoverImage { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool IsPublished { get; set; }
        public System.DateTime DatePublished { get; set; }
        public Nullable<int> PublishedByUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }

}
