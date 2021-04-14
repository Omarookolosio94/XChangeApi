using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Buyers
    {
        public int BuyerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public int MembershipId { get; set; }
        public int UserId { get; set; }
        public string Phone { get; set; }
    }
}
