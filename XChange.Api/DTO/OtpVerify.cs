using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XChange.Api.DTO
{
    public class OtpVerify
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

    public class ResetPassword
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
}
