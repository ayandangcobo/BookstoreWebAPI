using System.ComponentModel.DataAnnotations;
using System.Web;

namespace BookStoreDomain.Models
{
    public class BookCreateViewModel
    {
        [Required(ErrorMessage = "Book title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Book description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Book author is required")]
        public int Author { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Book price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Book price value must be greater than {1}")]
        public decimal Price { get; set; }

    }
}
