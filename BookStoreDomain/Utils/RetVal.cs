using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDomain
{
    public class RetVal<T> where T : class
    {
        public List<T> results { get; set; }
        public int count { get; set; }
        public List<string> errors { get; set; }
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public RetVal()
        {
            results = new List<T>();
            errors = new List<string>();
            count = 0;
            Message = string.Empty;
            IsSuccess = false;
        }

    }

}
