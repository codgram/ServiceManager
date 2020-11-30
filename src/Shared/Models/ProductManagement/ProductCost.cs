using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.ProductManagement
{   
    public class ProductCost
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ProductCostId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime CostDate { get; set; }

        public string VariantId { get; set; }
        public Variant Variant { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }
        public Currency Currency { get; set; }

    }
}