using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Offers
    {
        public int OfferId { get; set; }
        public int ProductId { get; set; }
        public int DiscountId { get; set; }
    }
}
