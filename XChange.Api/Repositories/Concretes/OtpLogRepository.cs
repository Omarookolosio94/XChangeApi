using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.Models;
using XChange.Api.Repositories.Interfaces;

namespace XChange.Api.Repositories.Concretes
{
    public class OtpLogRepository : BaseRepository<Models.OtpLog>, IOtpLogRepository
    {

        private static string ModuleName = "OtpLogRepository";

        public OtpLogRepository(XChangeDatabaseContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<bool> AddOtp(OtpLog otpLog)
        {
            try
            {
                await InsertAsync(otpLog);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                //log exception
                new Logger().LogError(ModuleName, "AddOtp", "Error Adding Otp" + ex + "\n");
                throw;
            }
        }

        public async Task<OtpLog> GetOtpByEmail(string email)
        {
            try
            {
                return Query().Where(o => o.Email == email).LastOrDefault();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetOtpByEmail", "Error Fetching Otp by Email " + email + ex + "\n");
                throw;
            }
        }

        public async Task<List<OtpLog>> GetOtpLogs()
        {
            try
            {
                return Query().OrderByDescending(otpLog => otpLog.OtpLogId).ToList();

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "GetOtpLogs", "Error Fetching All Otp Logs" + ex + "\n");
                throw;
            }
        }


        public async Task<bool> UpdateOtp(OtpLog otpLog)
        {
            try
            {
                Update(otpLog);
                await Commit();
                return true;
            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "UpdateOtp", "Error Updating Otp" + ex + "/n");
                throw;
            }
        }


        public async Task<bool> IsOtpValid(string email, string otp)
        {
            try
            {
                OtpLog userOtp = Query().Where(o => o.Email.ToString().ToLower() == email.ToString().ToLower() && o.Otp.ToString() == otp.ToString()).LastOrDefault();

                if (userOtp != null)
                {

                    if (!userOtp.IsValidated)
                    {
                        //update otp
                        userOtp.IsValidated = true;
                        Update(userOtp);
                        await Commit();

                        if (DateTime.Now.Subtract(userOtp.TimeSent ?? DateTime.Now).TotalMinutes <= 5)
                        {
                            return true;
                        }
                    }

                }

                return false;

            }
            catch (Exception ex)
            {
                new Logger().LogError(ModuleName, "IsOtpValid", "Error Validating otp " + otp + " for user email " + email + "error: " + ex + "/n");
                throw;
            }
        }
    }

}
