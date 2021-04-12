using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public DateTime? DateRegistered { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public bool IsLoggedIn { get; set; }
        public string Photo { get; set; }
    }
}
