using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using ServiceManager.Shared.Models.ProductManagement;

namespace ServiceManager.Shared.Models.Procurement
{
    public class PurchaseLine
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PurchaseLineId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string PurchaseHeaderId { get; set; }
        public PurchaseHeader PurchaseHeader { get; set; }

        public int LineNo { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LineDiscount { get; set; } // This is the discount amount and not the percentage
        public decimal TotalCost { 
            
            get {
                return (Quantity * UnitCost) - LineDiscount;
            }
        }


    }
}