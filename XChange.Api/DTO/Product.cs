using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class Product
    {
        public string Category { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductDescription { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsInOrder { get; set; }
        public string Picture { get; set; }
    }
}
