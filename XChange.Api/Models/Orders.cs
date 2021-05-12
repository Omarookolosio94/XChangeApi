using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Orders
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int? ShipperId { get; set; }
        public string OrderStatus { get; set; }
        public string CancelReason { get; set; }
        public string BillingPhone { get; set; }
        public int BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public string IpAddress { get; set; }
        public string Source { get; set; }
        public string Tag { get; set; }
        public string PaymentStatus { get; set; }
        public string Currency { get; set; }
        public decimal SubtotalPrice { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal TotalPrice { get; set; }
        public int? TotalWeight { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string ProductsId { get; set; }
        public string Summary { get; set; }
    }
}
