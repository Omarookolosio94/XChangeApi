using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class GiftCards
    {
        public int GiftCardId { get; set; }
        public string GiftCardNumber { get; set; }
        public string GiftCardExpiryMonth { get; set; }
        public string GiftCardExpiryYear { get; set; }
        public int PaymentId { get; set; }
    }
}
