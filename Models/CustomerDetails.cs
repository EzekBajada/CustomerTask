using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTask.Models
{
    public class CustomerDetails
    {
        public enum Countries { Malta, England, Italy, Greece}

        [Required]
        public Countries Country { get; set;}

        /// <summary>
        /// This determines if the employee is an active employee or inactive
        /// </summary>
        [Required]
        public bool Activity { get; set; }
        
        
    }
}
