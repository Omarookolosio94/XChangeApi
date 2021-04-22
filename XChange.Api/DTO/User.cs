using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class User
    {
        public string UserType { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class UserUpdate
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Gender { get; set; }
    }

}
