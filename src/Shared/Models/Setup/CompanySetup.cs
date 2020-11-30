using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.Setup
{
    public enum CompanySetupType {
        SendGrid
    }
    public class CompanySetup
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CompanySetupId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public CompanySetupType Type { get ;set; }
        public string Content { get; set; }


    }
}