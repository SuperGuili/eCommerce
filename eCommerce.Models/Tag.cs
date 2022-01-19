using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tag Name")]
        public string TagName { get; set; }

        [Display(Name = "Created Tag Date")]
        public DateTime TagCreatedDateTime { get; set; } = DateTime.Now;

        [Display(Name = "Edited Tag Date")]
        public DateTime TagEditedDateTime { get; set; } = DateTime.Now;

    }
}
