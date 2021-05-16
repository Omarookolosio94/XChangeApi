using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilities.Validation;
using XChange.Api.DTO;
using XChange.Api.Models;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;
using static XChange.Api.DTO.ModelError;
using XChange.Api.Services.Concretes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace XChange.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly IRegistrationLogService _registrationLogService;
        private readonly IAuditLogService _auditLogService;
        public readonly IEmailService _emailService;
        private readonly IOtpLogService _otpLogService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JWTSettings _jwtSettings;


        public UsersController(IEmailService emailService, IOptions<JWTSettings> jwtSettings)
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _registrationLogService = new RegistrationLogService(new RegistrationLogRepository(dbContext));
            _otpLogService = new OtpLogService(new OtpLogRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _refreshTokenService = new RefreshTokenService(new RefreshTokenRepository(dbContext));
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;

        }


        /// <summary>
        /// Get details of all registered users
        /// </summary>
        /// <returns>List of all users</returns>
        /// <response code="200">List of all users with hashed passwords</response>
        /// <response code="500">Server error message</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult users()
        {
            var result = _usersService.GetUsers();
            ApiResponse response;

            if (result.Result != null)
            {
                //Mask User Password
                var userList = result.Result;
                foreach (var user in userList)
                {
                    user.Password = "*********";
                }

                return Ok(userList);
            }
            else
            {
                response = new ApiResponse(500, "An error ocurred, please try again");
                return NotFound(response);
            }

        }

        /// <summary>
        /// Get Count of all registered users
        /// </summary>
        /// <returns>Count of all users</returns>
        /// <response code="200">Count of all users</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count" , Name ="GetUsersCount")]
        [AllowAnonymous]
        public async Task<IActionResult> Count()
        {
            var count = await _usersService.GetUsersCount();
            return Ok(count);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/users
        ///     
        ///     {
        ///        "Email": "johndoe@gmail.com",
        ///        "UserType": "B",
        ///        "Password": "johndoe_987"
        ///     }
        ///
        /// </remarks>
        /// <returns>Success message</returns>
        /// <response code="200">Registration success message , sends email to user</response>
        /// <response code="400">Pass in required information , error list</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Users([FromBody] User user)
        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            bool dataValid = true;

            //Validate User Type
            if (Validation.IsNull(user.UserType) || user.UserType.Length > 1 || !user.UserType.ToString().ToLower().Intersect("bs").Any())
            {
                Error err = new Error
                {
                    modelName = "UserType",
                    modelErrorMessgae = "Please , Select either B(Buyer) or S(Seller)",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate User Password
            if (Validation.IsNull(user.Password))
            {
                Error err = new Error
                {
                    modelName = "Password",
                    modelErrorMessgae = "Password is required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate User Email
            if (Validation.IsNull(user.Email))
            {
                Error err = new Error
                {
                    modelName = "Email",
                    modelErrorMessgae = "Email is required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate User Email
            if (!Validation.IsNull(user.Email) && !Validation.IsValidEmail(user.Email))
            {
                Error err = new Error
                {
                    modelName = "Email",
                    modelErrorMessgae = "Email format not valid",
                };
                errorList.Add(err);
                dataValid = false;
            }

            //check if email has been registered
            bool isUserRegistered = await _usersService.IsEmailRegistered(user.Email);

            if (isUserRegistered)
            {
                Error err = new Error
                {
                    modelName = "Email",
                    modelErrorMessgae = "Email already Registered",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Pass Error Response
            if (!dataValid)
            {
                errors = new ModelError(400, "Pass in Required Information", errorList);

                //log errors
                RegistrationLog registrationErrorLog = Utility.Utility.AddRegistrationLog(user, false, errors.ToString());
                _registrationLogService.AddRegistrationLog(registrationErrorLog);

                return BadRequest(errors);
            }

            //create Users model
            Users newUser = new Users
            {
                Password = BC.HashPassword(user.Password),
                DateRegistered = DateTime.Now,
                UserType = user.UserType.ToUpper(),
                Email = user.Email.ToString().ToLower(),
            };

            var result = await _usersService.RegisterUser(newUser);

            if (result)
            {
                //log successful request
                RegistrationLog registrationSuccessLog = Utility.Utility.AddRegistrationLog(user, true);
                _registrationLogService.AddRegistrationLog(registrationSuccessLog);

                //generate otp
                OtpLog userOtp = Utility.Utility.NewOtpLog(user.Email);

                //save otp to database
                var otpResult = await _otpLogService.AddOtp(userOtp);
                Message message;

                //send otp validation email to user
                if (otpResult)
                {
                    message = new Message(new string[] { user.Email }, "Registration Successful", "Your Registration to XChange.com was successful. Validate your account using otp: " + userOtp.Otp);
                }
                else
                {
                    message = new Message(new string[] { user.Email }, "Registration Successful", "Your Registration to XChange.com was successful. Visit XChange.com/Validate to validate your account");
                }

                _emailService.SendEmail(message);

                ApiResponse response = new ApiResponse(200, "Registration Successful", "Verfification Email has been sent to " + user.Email);
                return Ok(response);
            }
            else
            {
                ApiResponse response = new ApiResponse(400, "An error occurred while processing information provided. Please , review Details and try again");
                return BadRequest(response);
            }
        }


        /// <summary>
        /// Get single user 
        /// </summary>
        /// <returns>Details of user</returns>
        /// <response code="200">Details of user with hashed password</response>
        /// <response code="404">user not found</response>
        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [AllowAnonymous]
        public IActionResult GetUser(int userId)
        {

            var result = _usersService.GetUser(userId);
            ApiResponse response;

            if (result.Result != null)
            {
                //Mask User Password
                result.Result.Password = "********";

                return Ok(result.Result);
            }
            else
            {
                response = new ApiResponse(404, "User not found");
                return NotFound(response);
            }

        }

        /// <summary>
        /// Verify Otp issued to user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/users/otp
        ///     
        ///     {
        ///        "Email": "test@gmail.com",
        ///        "Otp": "123456",
        ///     }
        ///
        /// </remarks>
        /// <returns>Success message</returns>
        /// <response code="200">Your account has been verified  successfully</response>
        /// <response code="400">An error occurred, please try again</response>
        [HttpPost("otp", Name = "VerifyOtp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Otp([FromBody] OtpVerify otpVerify)
        {

            //validate otp
            var isOtpValid = await _otpLogService.IsOtpValid(otpVerify.Email, otpVerify.Otp);
            ApiResponse response;

            if (isOtpValid)
            {
                //update user verified status if successfull
                var user = await _usersService.GetUserByEmail(otpVerify.Email);

                if (user != null)
                {
                    if (!user.IsVerified)
                    {
                        await _usersService.VerifyUser(user.UserId, true);
                    }

                    //Generate refresh Token
                    RefreshToken refreshToken = Utility.Utility.GenerateRefreshToken(user.UserId);
                    await _refreshTokenService.AddRefreshToken(refreshToken);

                    //Generate Access Token
                    var token = GenerateAccessToken(user);

                    TokenResponse tokenResponse = new TokenResponse
                    {
                        Token = token,
                        RefreshToken = refreshToken.Token,
                        Email = user.Email,
                        UserId = user.UserId,
                        UserType = user.UserType
                    };

                    //response = new ApiResponse(200, "Your account has been verified successfully");
                    return Ok(tokenResponse);

                }
                else
                {
                    response = new ApiResponse(400, "An error occurred, Please Try Again");
                    return NotFound(response);
                }

            }
            else
            {
                response = new ApiResponse(400, "An error occured, request for new OTP");
                return BadRequest(response);
            }

        }



        /// <summary>
        /// Generate Otp for user
        /// </summary>
        /// <returns>Otp generation success message</returns>
        /// <response code="200">Otp success , validation Otp sent to user email</response>
        /// <response code="400">One or more error occured, request for new Otp</response>
        [HttpGet("otp", Name = "GenerateOtp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Otp(string email)
        {
            ApiResponse response;

            //validate email
            if (Validation.IsNull(email) || !Validation.IsValidEmail(email))
            {
                List<Error> errorList = new List<Error> { };
                Error err = new Error
                {
                    modelName = "Email",
                    modelErrorMessgae = "Valid Email is Required",
                };

                errorList.Add(err);
                ModelError errors = new ModelError(400, "Pass in Required Information", errorList);
                return BadRequest(errors);
            }

            //check if email has been registered
            bool isUserRegistered = await _usersService.IsEmailRegistered(email);
            if (!isUserRegistered)
            {
                response = new ApiResponse(400, "Account not yet registered , Please register");
                return NotFound(response);
            }

            var userEmail = email.ToString().ToLower();

            //generate otp
            OtpLog userOtp = Utility.Utility.NewOtpLog(userEmail);

            //save otp to database
            bool otpResult = await _otpLogService.AddOtp(userOtp);

            if (otpResult)
            {
                var message = new Message(new string[] { userEmail }, "Validation OTP", "Validate your account using otp: " + userOtp.Otp);
                _emailService.SendEmail(message);
                response = new ApiResponse(200, "OTP Generated successfully", "Validation OTP has been sent to: " + userEmail);
                return Ok(response);
            }

            response = new ApiResponse(400, "An error occured, request for new OTP");
            return BadRequest(response);
        }



        /// <summary>
        /// Reset Password ,sends reset Otp to user
        /// </summary>
        /// <returns>Password reset otp generation success message</returns>
        /// <response code="200">Otp success , password reset Otp sent to user email</response>
        /// <response code="400">One or more error occured, request for new Otp</response>
        [HttpGet("password", Name = "ResetPasswordOTP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Password(string email)
        {
            ApiResponse response;

            //validate email
            if (Validation.IsNull(email) || !Validation.IsValidEmail(email))
            {
                List<Error> errorList = new List<Error> { };
                Error err = new Error
                {
                    modelName = "Email",
                    modelErrorMessgae = "Valid Email is Required",
                };

                errorList.Add(err);
                ModelError errors = new ModelError(400, "Pass in Required Information", errorList);
                return BadRequest(errors);
            }

            //check if email has been registered
            bool isUserRegistered = await _usersService.IsEmailRegistered(email);

            if (!isUserRegistered)
            {
                response = new ApiResponse(400, "Account not yet registered , Please register");
                return NotFound(response);
            }

            var userEmail = email.ToString().ToLower();
            //generate otp
            OtpLog userOtp = Utility.Utility.NewOtpLog(userEmail);
            //save otp to database
            bool otpResult = await _otpLogService.AddOtp(userOtp);

            if (otpResult)
            {
                var message = new Message(new string[] { userEmail }, "Reset Password", "Rest  your password using otp: " + userOtp.Otp);
                _emailService.SendEmail(message);
                response = new ApiResponse(200, "OTP Generated successfully", "OTP to reset password has been sent to: " + userEmail);
                return Ok(response);
            }

            response = new ApiResponse(400, "An error occured, try Again");
            return BadRequest(response);
        }


        /// <summary>
        /// Reset password
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/users/password
        ///     
        ///     {
        ///        "Email": "test@gmail.com",
        ///        "Otp": "123456",
        ///        "NewPassword": "newtest"
        ///     }
        ///
        /// </remarks>
        /// <returns>Success message</returns>
        /// <response code="200">Your password has been updated</response>
        /// <response code="400">An error occurred, request for new password reset Otp</response>
        [HttpPut("password", Name = "ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Password([FromBody] ResetPassword resetPassword)
        {
            ApiResponse response;

            var otp = resetPassword.Otp;
            var password = BC.HashPassword(resetPassword.NewPassword);
            var email = resetPassword.Email.ToString().ToLower();

            var isOtpValid = await _otpLogService.IsOtpValid(email, otp);

            if (isOtpValid)
            {
                var result = await _usersService.ResetPassword(email, password);

                if (result)
                {

                    //add audit log
                    AuditLog auditLog = Utility.Utility.AddAuditLog(email, "Reset Password Detail");
                    _auditLogService.AddAuditLog(auditLog);

                    response = new ApiResponse(200, "Your Password has been updated");
                    return Ok(response);

                }
                else
                {
                    response = new ApiResponse(400, "An error occurred, Please Request for New Password Reset OTP");
                    return BadRequest(response);
                }

            }
            else
            {
                response = new ApiResponse(400, "An error occured, Request for New Password reset OTP");
                return BadRequest(response);
            }

        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/users/token
        ///     
        ///     {
        ///        "Token": " ",
        ///        "RefreshToken": " ",
        ///     }
        ///
        /// </remarks>
        /// <returns>Success message</returns>
        /// <response code="200">Returns token response</response>
        /// <response code="400">An error occurred, please try again</response>
        [HttpPost("token", Name = "RefreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            ApiResponse response;
            //Get user from access token
            Users user = await GetUserFromAccessToken(refreshTokenRequest.Token);

            bool isRefreshTokenValid = await _refreshTokenService.IsRefreshTokenValid(user.UserId, refreshTokenRequest.RefreshToken);

            if (user != null && isRefreshTokenValid)
            {
                //Generate Access Token
                var token = GenerateAccessToken(user);

                TokenResponse tokenResponse = new TokenResponse
                {
                    Token = token,
                    RefreshToken = refreshTokenRequest.RefreshToken,
                    Email = user.Email,
                    UserId = user.UserId,
                    UserType = user.UserType
                };

                return Ok(tokenResponse);
            }

            response = new ApiResponse(400, "An error occurred, Please Try Again");
            return NotFound(response);
        }

        private async Task<Users> GetUserFromAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken securityToken;
                var principle = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principle.FindFirst(ClaimTypes.Name)?.Value;
                    return await _usersService.GetUser(Convert.ToInt32(userId));
                }
            }
            catch (Exception)
            {
                return new Users();
            }

            return new Users();
        }
    
        private string GenerateAccessToken(Users user)
        { 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim(ClaimTypes.Name , Convert.ToString(user.UserId)),
                            new Claim(ClaimTypes.Role, user.UserType),
                            new Claim(ClaimTypes.Email , user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
