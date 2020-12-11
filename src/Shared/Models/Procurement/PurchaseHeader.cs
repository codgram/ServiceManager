using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.Procurement
{
    public class PurchaseHeader
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PurchaseHeaderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public string PurchaseOrderNo { get; set; } //Starting 0000001
        public string VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public bool IsPosted { get; set; }


    }
}