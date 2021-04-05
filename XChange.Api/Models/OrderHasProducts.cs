using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class OrderHasProducts
    {
        public int OrderProductId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
