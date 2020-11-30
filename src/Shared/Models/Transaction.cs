using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models
{
    
    public class Transaction
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TransactionId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string CompanyId { get; set; }

        public DocumentType Type { get; set; }

        public string DocumentId { get; set; }
        public string ServiceManagerUserId { get; set; }
        public bool Posted { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitAmount { get; set; } //Retrives the final Total Amount from the document line


        public decimal TotalAmount {
            get {
                return Quantity * UnitAmount;
            }
        }

    }
}