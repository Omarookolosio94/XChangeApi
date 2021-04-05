using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Payments
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public int OrderId { get; set; }
    }
}
