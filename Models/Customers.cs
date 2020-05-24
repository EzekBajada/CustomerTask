using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTask.Models
{
    public class Customers
    {
        public enum Countries { Malta, England, Italy, Greece }

        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(150)]
        public string Position { get; set; }

        [Required]
        public Countries Country { get; set; }

        /// <summary>
        /// This determines if the employee is an active employee or inactive
        /// </summary>
        public bool Activity { get; set; }
    }
}
    