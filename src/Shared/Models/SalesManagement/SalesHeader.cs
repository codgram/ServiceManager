using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.SalesManagement
{
    public enum PaymentStatus { 
        Pending,
        Partial,
        Paid
    }
    public class SalesHeader
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SalesHeaderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public DateTime DeliveryDate { get; set; }
        public DateTime PostedDate { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public string SalesOrderNo { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public bool IsShipped { get; set; }
        
        public bool IsPosted { get; set; }

        public PaymentStatus PaymentStatus { get; set; }


    }
}