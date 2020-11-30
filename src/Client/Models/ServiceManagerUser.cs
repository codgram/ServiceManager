using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceManager.Client.Models
{

    public enum UserType {
        Owner,
        Admin,
        User,
        Customer
    }

    public class ServiceManagerUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get {
                return FirstName + " " + LastName;
            }
        }

        public UserType Type { get; set; }
    }
}
