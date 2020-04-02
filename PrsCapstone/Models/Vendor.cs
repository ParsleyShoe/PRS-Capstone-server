using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrsCapstone.Models {
    public class Vendor {

        public int Id { get; set; }
        [StringLength(16)]
        [Required]
        public string Code { get; set; }
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
        [StringLength(80)]
        [Required]
        public string Address { get; set; }
        [StringLength(40)]
        [Required]
        public string City { get; set; }
        [StringLength(2)]
        [Required]
        public string State { get; set; }
        [StringLength(5)]
        [Required]
        public string ZIP { get; set; }
        [StringLength(10)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Email { get; set; }

    }
}
