using System;

namespace BookStoreDomain.Models
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string BookDescription { get; set; }
        public UserViewModel Author { get; set; }
        public string CoverImage { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool IsPublished { get; set; }
        public System.DateTime DatePublished { get; set; }
        public UserViewModel PublishedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
