using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class ShoppingCart
    {
        public Product Product { get; set; }

        [Range(1,99, ErrorMessage ="Please enter a quantity between 1 and 99.")]
        public int Count { get; set; }
    }
}
