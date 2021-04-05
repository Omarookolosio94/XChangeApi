using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class WishLists
    {
        public int WishListId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
