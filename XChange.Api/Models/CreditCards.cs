using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class CreditCards
    {
        public int CreditCardId { get; set; }
        public string CreditCardNumber { get; set; }
        public int CardCvv { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string CardAddress { get; set; }
        public string CardCity { get; set; }
        public string CardPostalCode { get; set; }
        public int PaymentId { get; set; }
    }
}
