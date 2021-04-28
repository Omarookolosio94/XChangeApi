using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Products
    {
        public int ProductId { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int? Ranking { get; set; }
        public string ProductDescription { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsInOrder { get; set; }
        public string PictureName { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? TimeAdded { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int SellerId { get; set; }
    }
}
