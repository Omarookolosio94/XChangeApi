using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class RegistrationLog
    {
        public int RegistrationLogId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Error { get; set; }
        public bool? IsSuccessful { get; set; }
        public DateTime? TimeLogged { get; set; }
    }
}
