using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class OtpLogService :  IOtpLogService
    {

        private readonly IOtpLogRepository _otpLogRepository;

        public OtpLogService(IOtpLogRepository otpLogRepository)
        {
            _otpLogRepository = otpLogRepository;

        }

        public async Task<bool> AddOtp(OtpLog otpLog)
        {
            try
            {
                var status = await _otpLogRepository.AddOtp(otpLog);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }


        public async Task<List<OtpLog>> GetOtpLogs()
        {
            try
            {
                var status = await _otpLogRepository.GetOtpLogs();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OtpLog> GetOtpByEmail(string email)
        {
            try
            {
                OtpLog userOtp = await _otpLogRepository.GetOtpByEmail(email);
                return userOtp;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> IsOtpValid(string email , string otp)
        {
            try
            {
                bool isOtpValid = await _otpLogRepository.IsOtpValid(email, otp);
                return isOtpValid;

            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<bool> UpdateOtp(OtpLog otpLog)
        {
            try
            {
                return await _otpLogRepository.UpdateOtp(otpLog);
            } catch(Exception ex)
            {
                throw;
            }
        }
    }
}
