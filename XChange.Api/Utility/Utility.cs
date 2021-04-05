using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.DTO;
using XChange.Api.Models;
using BC = BCrypt.Net.BCrypt;


namespace XChange.Api.Utility
{
    public class Utility
    {
        public static RegistrationLog AddRegistrationLog(User user, bool isSuccess, string errors = " ")
        {

            RegistrationLog registrationLog = new RegistrationLog
            {
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserType = user.UserType,
                Password = BC.HashPassword(user.Password),
                Gender = user.Gender,
                IsSuccessful = isSuccess,
                Error = errors,
                TimeLogged = DateTime.Now,
            };

            return registrationLog;

        }
    }
}
