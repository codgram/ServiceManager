using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.ProductManagement
{
    public enum VariantType{ 
        Regular,
        Color,
        Edition,
        Flavor,
        Format,
        Length,
        Material,
        Model,
        Platform,
        Scent,
        Size,
        Width

    }
    public class Variant
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string VariantId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string VariantNo { get; set; }

        public VariantType Type { get; set; }
        public string Variation { get; set; }
    }
}