using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServiceManager.Shared.Models.ProductManagement;

namespace ServiceManager.Shared.Models.SalesManagement
{
    public class InvoiceLine
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string InvoiceLineId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string InvoiceHeaderId { get; set; }
        public InvoiceHeader InvoiceHeader { get; set; }

        public int LineNo { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public string VariantId { get; set; }
        public Variant Variant { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LineDiscount { get; set; } // This is the discount amount and not the percentage
        
        public decimal TotalPrice { 
            
            get {
                return (Quantity * UnitPrice) - LineDiscount;
            }
        }


    }
}