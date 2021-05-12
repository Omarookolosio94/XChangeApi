using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Carts
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
