using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrsCapstone.Models {
    public class RequestLine {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RequestId { get; set; }
        [Min(0)]
        public int Quantity { get; set; } = 1;

        public virtual Product Product { get; set; }
        [JsonIgnore]
        public virtual Request Request { get; set; }

    }
}
