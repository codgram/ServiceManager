using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.Setup
{
    public enum DocumentSetupPart {
        Prefix,
        Suffix
    }
    public class DocumentSetup
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DocumentSetupId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public DocumentType DocumentType { get ;set; } //Purchase, Sale, Invoice
        public DocumentSetupPart Part { get; set; } //Prefix or Suffix
        public string Content { get; set; } //PO-CO-


    }
}