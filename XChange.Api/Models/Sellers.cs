﻿using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Sellers
    {
        public int SellerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPosition { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public bool? IsLoggedIn { get; set; }
        public int UserId { get; set; }
    }
}
