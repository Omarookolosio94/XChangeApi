using System;
using System.Collections.Generic;

namespace XChange.Api.Models
{
    public partial class OtpLog
    {
        public long OtpLogId { get; set; }
        public string Otp { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool? IsSent { get; set; }
        public bool IsValidated { get; set; }
        public DateTime? TimeSent { get; set; }
    }
}
