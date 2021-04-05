using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public string AdressType { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public int UserId { get; set; }
    }
}
