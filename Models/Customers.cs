using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTask.Models
{
    public class Customers
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(150)]
        public string Position { get; set; }

        [Required]
        public CustomerDetails Details { get; set; }
    }
}
