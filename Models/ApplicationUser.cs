using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Film.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
   
    public class ApplicationUser : IdentityUser
    {
        public bool Blocked { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
    }
}
