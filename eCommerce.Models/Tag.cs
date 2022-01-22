using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tag Name")]
        public string TagName { get; set; }

        [Required]
        [Display(Name = "Tag Discount")]
        public double TagDiscountPCent { get; set; } = 0;

        [Display(Name = "Created Tag Date")]
        public DateTime TagCreatedDateTime { get; set; } = DateTime.Now;

        [Display(Name = "Edited Tag Date")]
        public DateTime TagEditedDateTime { get; set; } = DateTime.Now;

    }
}
