using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Validation
{
    public class Validation
    {
        public static bool IsNull(string parameter)
        {
            return (parameter == null || parameter == String.Empty) ? true : false;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException ex)
            {
                return false;
            }
            catch (ArgumentException ex)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string GenerateOTP(int otpDigits = 6)
        {
            string number = "0123456789";
            int length = number.Length;
            string otp = string.Empty;
            //int otpDigits = 5;
            string finalDigits;
            int getIndex;

            for(int i =0; i< otpDigits; i++)
            {
                do
                {
                    getIndex = new Random().Next(0, length);
                    finalDigits = number.ToCharArray()[getIndex].ToString();
                     
                } while (otp.IndexOf(finalDigits)!=-1);
                otp += finalDigits;
            }

            return otp;
        }

    }
}
