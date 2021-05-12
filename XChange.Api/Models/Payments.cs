using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Payments
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string IpAddress { get; set; }
        public string Source { get; set; }
        public string Location { get; set; }
        public string Receipt { get; set; }
        public string Message { get; set; }
        public string ErrorCodes { get; set; }
        public string Status { get; set; }
    }
}
