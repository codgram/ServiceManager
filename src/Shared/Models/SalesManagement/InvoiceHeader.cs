using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.SalesManagement
{
    public class InvoiceHeader
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string InvoiceHeaderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string InvoiceOrderNo { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public bool Posted { get; set; }

    }
}