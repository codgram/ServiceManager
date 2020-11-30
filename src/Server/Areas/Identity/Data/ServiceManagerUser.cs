using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ServiceManager.Server.Areas.Identity.Data
{

    public enum UserType {
        Owner,
        Admin,
        User,
        Customer
    }
    // Add profile data for application users by adding properties to the ServiceManagerUser class
    public class ServiceManagerUser : IdentityUser
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get {
                return FirstName + " " + LastName;
            }
        }

        public UserType Type { get; set; }
    }
}
