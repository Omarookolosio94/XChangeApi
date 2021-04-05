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
using XChange.Data.Services.Concretes;
using BC = BCrypt.Net.BCrypt;
using static XChange.Api.DTO.ModelError;
using XChange.Api.Services.Concretes;

namespace XChange.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly IRegistrationLogService _registrationLogService;
        public readonly IEmailService _emailService;


        public UsersController(IEmailService emailService)
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _registrationLogService = new RegistrationLogService(new RegistrationLogRepository(dbContext));
            _emailService = emailService;
          
        }

        //POST api/users
        //[ProducesResponseType(201)]
        //[ProducesResponseType(400)]
        //[Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Users([FromBody] User user)

        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            //RegistrationLog registrationLog = new RegistrationLog { };
            bool dataValid = true;

            //Validate First Name
            if (Validation.IsNull(user.UserFirstName))
            {
                Error err = new Error
                {
                    modelName = "UserFirstName",
                    modelErrorMessgae = "First Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate FirstName Length
            if (!Validation.IsNull(user.UserFirstName) && user.UserFirstName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "UserFirstName",
                    modelErrorMessgae = "First Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate Last Name
            if (Validation.IsNull(user.UserLastName))
            {
                Error err = new Error
                {
                    modelName = "UserLastName",
                    modelErrorMessgae = "Last Name is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }


            //Validate FirstName Length
            if (!Validation.IsNull(user.UserLastName) && user.UserLastName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "UserLastName",
                    modelErrorMessgae = "Last Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

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

            //Hash Password , Override Date to present Date , Change UserType and Gender to Uppercases
            Users newUser = new Users
            {
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                Password = BC.HashPassword(user.Password),
                DateRegistered = DateTime.Now,
                UserType = user.UserType.ToUpper(),
                Gender = user.Gender,
                Email = user.Email,
            };

            if (!Validation.IsNull(newUser.Gender))
            {
                newUser.Gender = newUser.Gender.ToUpper();
            }

            await _usersService.RegisterUser(newUser);

            //log successful request
            RegistrationLog registrationSuccessLog = Utility.Utility.AddRegistrationLog(user, true);
            _registrationLogService.AddRegistrationLog(registrationSuccessLog);


            var message = new Message(new string[] { user.Email}, "Registration Successful", "You have been successful registered");
            _emailService.SendEmail(message);

            return CreatedAtRoute("GetUser", new { UserId = newUser.UserId }, newUser);
        }



        [HttpGet("{userId}", Name = "GetUser")]
        public IActionResult GetUser(int userId)
        {

            var result = _usersService.GetUser(userId);
            ApiError response;

            if (result.Result != null)
            {
                return Ok(result.Result);
            }
            else
            {
                response = new ApiError(404, "Not Found");
                return NotFound(response);
            }

        }


        [HttpGet]
        public IActionResult Users()
        {

            var result = _usersService.GetUsers();
            ApiError response;

            if (result.Result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiError(404, "No User Found");
                return NotFound(response);
            }

        }

    }
}
