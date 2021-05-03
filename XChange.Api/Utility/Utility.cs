using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
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

        public static AuditLog AddAuditLog(int userId, string email, string activity = " ")
        {

            AuditLog auditLog = new AuditLog
            {
                Activity = activity,
                Email = email,
                TimeLogged = DateTime.Now,
                UserId = userId
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

        public static RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }


        public static RefreshToken GenerateRefreshToken(int userId)
        {
            RefreshToken refreshToken = new RefreshToken();
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);
            refreshToken.UserId = userId;

            return refreshToken;
        }

        public static string CalculateProductRating(string currentRating , int rating)
        {
            double doubleRating = Convert.ToDouble(currentRating);
            var totalRating = doubleRating + rating;
            var newRating = totalRating / (double)2;

            newRating = Math.Round(newRating, 2);
            return newRating.ToString();
        }

        public static string CalculatePreviousRating(string currentRating , string rating)
        {
            //rating is the value for previous rating
            //current rating is value of current product rating

            double doubleRating = Convert.ToDouble(currentRating);
            doubleRating = doubleRating * 2;
            var previousRating = doubleRating - Convert.ToDouble(rating);

            previousRating = Math.Round(previousRating, 2);
            return previousRating.ToString();
        }

        public static string CalculateUpdateRating(string productRating , string previousRating , string currentRating)
        {
            //product rating is the value of current product rating
            //previous rating is the current value of user rating on product
            //current rating is the new value of user rating on product

            string revertRating = CalculatePreviousRating(productRating, previousRating);

            string updateRating = CalculateProductRating(revertRating , Convert.ToInt32(currentRating));

            return updateRating;
        }
    }
}
