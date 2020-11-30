using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.SalesManagement
{
    public class SalesHeader
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SalesHeaderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public string SalesOrderNo { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public bool Posted { get; set; }


    }
}