using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Orders
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public decimal? Freight { get; set; }
        public decimal? SalesTax { get; set; }
        public byte[] TimeStamp { get; set; }
        public string TransactStatus { get; set; }
        public int? InvoiceAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? ItemQuantity { get; set; }
        public int UserId { get; set; }
        public int ShipperId { get; set; }
    }
}
