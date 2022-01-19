using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        [Range(0,10000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(0, 10000)]
        public double Price { get; set; }

        [Required]
        [Range(0, 10000)]
        public double Price10 { get; set; }

        [Required]
        [Range(0, 10000)]
        public double Price20 { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public Tag  Tag { get; set; }
    }
}
