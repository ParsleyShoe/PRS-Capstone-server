using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrsCapstone.Models {
    public class Request {

        public int Id { get; set; }
        [StringLength(80)]
        [Required]
        public string Description { get; set; }
        [StringLength(80)]
        [Required]
        public string Justification { get; set; }
        [StringLength(80)]
        public string RejectionReason { get; set; }
        [StringLength(20)]
        [Required]
        public string DeliveryMode { get; set; }
        [StringLength(10)]
        [Required]
        public string Status { get; set; } = "NEW";
        public decimal Total { get; set; } = 0;
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual List<RequestLine> Requestlines { get; set; }

    }
}
