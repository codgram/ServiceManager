using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models
{

    public enum Currency { 
        LBP
    }

    public enum DocumentType {
        Purchase,
        Sales,
        Invoice
    }

    public class Company
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CompanyId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Slug is too long.")]
        [RegularExpression(@"^[a-z''-'\s]{1,20}$", ErrorMessage = "Characters are not allowed.")]
        public string Slug { get; set; } //The unique name and url path of the company

        public string ServiceManagerUserId { get; set; } //Created by user

    }
}