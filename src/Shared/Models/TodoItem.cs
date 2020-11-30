using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceManager.Shared.Models
{

    public class TodoItem
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TodoItemId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string CompanyId { get; set; }
        public Company Company { get ;set; }
        
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        //[Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string ServiceManagerUserId { get; set; } //Assigned to

        public bool IsCompleted { get; set; }

    }
}