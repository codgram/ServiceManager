using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.ProductManagement
{
    public class ProductSubcategory
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ProductSubcategoryId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        [Required]
        public string ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}