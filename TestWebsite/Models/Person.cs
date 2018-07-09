using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebsite.Models
{
    public class Person
    {

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Job { get; set; }
        
    }
}
