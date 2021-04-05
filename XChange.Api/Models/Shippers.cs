using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Shippers
    {
        public int ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
    }
}
