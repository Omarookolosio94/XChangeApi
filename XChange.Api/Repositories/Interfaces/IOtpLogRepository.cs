using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;

namespace XChange.Api.Repositories.Interfaces
{
    public interface IOtpLogRepository
    {
        Task<bool> AddOtp(OtpLog otpLog);
        Task<List<OtpLog>> GetOtpLogs();
        Task<OtpLog> GetOtpByEmail(string email);
        Task<bool> UpdateOtp(OtpLog otpLog);
        Task<bool> IsOtpValid(string email, string otp);
    }
}
