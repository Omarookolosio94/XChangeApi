using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class Order
    {
        public string Billing_Address { get; set; }
        public string Billing_Phone { get; set; }
        public string Summary { get; set; }
        public string Tag { get; set; }
        public bool UseSavedAddress { get; set; } //default false
    }
}
