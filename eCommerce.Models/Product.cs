using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [Range(0.01,10000)]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Price for 1-10")]
        public double Price { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Price for 10-20")]
        public double Price10 { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Price for 20+")]
        public double Price20 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [Display(Name ="Tag")]
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        [ValidateNever]
        public Tag  Tag { get; set; }
    }
}
