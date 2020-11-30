using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.ProductManagement
{
    public class VariantImage
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string VariantImageId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        
        public string VariantId { get; set; }
        public Variant Variant { get; set; }

        [Required]
        public string Color { get; set; } //Example: #F6F6F6

        [Url]
        [Required]
        public string ImageURL { get; set; }
    }
}