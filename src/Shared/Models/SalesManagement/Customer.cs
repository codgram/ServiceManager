using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models.SalesManagement
{
    public class Customer
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CustomerId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        //If the customer is a user that uses the client app
        public string ServiceManagerUserId { get; set; } 
        
        [Required]
        public string Name { get; set; }

        [Phone]
        public string PhoneNo { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        

    }
}