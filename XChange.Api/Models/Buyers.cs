using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Buyers
    {
        public int BuyerId { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public int? Phone { get; set; }
        public string Email { get; set; }
        public bool? IsLoggedIn { get; set; }
        public int MembershipId { get; set; }
        public int UserId { get; set; }
    }
}
