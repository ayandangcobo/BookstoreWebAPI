using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDomain
{
    public class DomainValidator
    {
        public static List<ValidationResult> ValidateModel<T>(T model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model),  results, true);
            return results;
        }
    }
}
