using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class ShoppingCarts
    {
        public int ShoppingCartId { get; set; }
        public string OrderStatus { get; set; }
        public int ProductId { get; set; }
    }
}
