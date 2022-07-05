using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDomain.Models
{
    public class BookSearchViewModel
    {
        public string title { get; set; }
        public string author { get; set; }
        public string description { get; set; }
    }
}
