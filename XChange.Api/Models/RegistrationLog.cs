using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class RegistrationLog
    {
        public int RegistrationLogId { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime? TimeLogged { get; set; }
    }
}
