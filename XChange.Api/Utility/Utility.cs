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
                UserType = user.UserType,
                Password = BC.HashPassword(user.Password),
                Email = user.Email,
                IsSuccessful = isSuccess,
                Error = errors,
                TimeLogged = DateTime.Now,
            };

            return registrationLog;

        }

        public static AuditLog AddAuditLog(int userId, string activity = " ")
        {

            AuditLog auditLog = new AuditLog
            {
                Activity = activity,
                UserId = userId,
                TimeLogged = DateTime.Now,
            };

            return auditLog;
        }

        public static AuditLog AddAuditLog(string email, string activity = " ")
        {

            AuditLog auditLog = new AuditLog
            {
                Activity = activity,
                Email = email,
                TimeLogged = DateTime.Now,
            };

            return auditLog;
        }

        public static OtpLog NewOtpLog(string email , int otpLength = 6)
        {
            var otp = Utilities.Validation.Validation.GenerateOTP(otpLength);

            OtpLog otpLog = new OtpLog
            {
                Email = email.ToString().ToLower(),
                IsSent = true,
                IsValidated = false,
                Otp = otp,
                TimeSent = DateTime.Now
            };

            return otpLog;

        }
    }
}
