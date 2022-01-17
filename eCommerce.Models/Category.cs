using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Display Order")]
        [Range(1,999, ErrorMessage ="Display Order must be between 1 and 999")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CategoryCreatedDateTime { get; set; } = DateTime.Now;

    }
}
