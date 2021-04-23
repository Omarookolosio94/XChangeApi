using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Validation;
using XChange.Api.DTO;
using XChange.Api.Models;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;
using XChange.Data.Services.Concretes;
using static XChange.Api.DTO.ModelError;

namespace XChange.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/account")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly IBuyersService _buyersService;
        private readonly IAuditLogService _auditLogService;


        public AccountController()
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _buyersService = new BuyersService(new BuyersRepository(dbContext));
        }


        /// <summary>
        /// Get account of all Buyers
        /// </summary>
        /// <returns>Details of buyer</returns>
        /// <response code="200">Details of all buyers</response>
        /// <response code="500">An error occured, please try again</response>
        [HttpGet(Name = "GetAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Account()
        {
            var result = _buyersService.GetBuyers();
            ApiResponse response;

            if (result.Result != null)
            {
                return Ok(result.Result);
            }
            else
            {
                response = new ApiResponse(500, "An error ocurred, please try again");
                return NotFound(response);
            }

        }



        /// <summary>
        /// Search for Buyers
        /// </summary>
        /// <returns>List of Buyers that matches request</returns>
        /// <response code="200">List of Buyers or match not found</response>
        /// <response code="500">An error occurred , try again</response>
        [HttpGet("search", Name = "SearchBuyers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> SearchAccount([FromQuery] string search)
        {

            var result = await _buyersService.SearchBuyers(search.ToString().ToLower());

            ApiResponse response;

            if (result != null)
            {
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "match not found");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(500, "An error occurred, try again");
                return NotFound(response);
            }

        }


        /// <summary>
        /// Get account of a buyer
        /// </summary>
        /// <returns>Details of a buyer</returns>
        /// <response code="200">Details of a buyer</response>
        /// <response code="404">Buyer not found</response>
        [HttpGet("{userId}", Name = "GetBuyer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public IActionResult GetBuyer(int userId)
        {

            var result = _buyersService.GetBuyer(userId);
            ApiResponse response;

            if (result.Result != null)
            {
                return Ok(result.Result);
            }
            else
            {
                response = new ApiResponse(404, "Account not found, Please Create one");
                return NotFound(response);
            }

        }


        /// <summary>
        /// Creates a new account for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/account/{userId}
        ///     
        ///     {
        ///        FirstName : "John",
        ///        LastName: "Doe",
        ///        CompanyName: "Maxwell Enterprise Inc",
        ///        Phone : "07019000010",
        ///        Gender: "Male"
        ///     }
        ///
        /// </remarks>
        /// <returns>New Buyer's account</returns>
        /// <response code="201">Newly created buyer's account</response>
        /// <response code="400">Pass in required information , create a seller account, account already exists</response>
        /// <response code="404">User's profile not found</response>
        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> Account(int userId, [FromBody] Buyer buyer)
        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            ApiResponse response;
            bool dataValid = true;

            //get user details
            Users user = await _usersService.GetUser(userId);

            if (user == null)
            {
                response = new ApiResponse(404, "User profile not found");
                return NotFound(response);
            }

            if (user.UserType.ToUpper() == "S")
            {
                response = new ApiResponse(400, "Please, create a seller's account");
                return BadRequest(response);
            }

            var isRegistered = await _buyersService.IsBuyerRegistered(userId);

            //check if user has an account already
            if (isRegistered)
            {
                response = new ApiResponse(400, "Account already exist");
                return BadRequest(response);
            }

            //Validate First Name
            if (Validation.IsNull(buyer.FirstName))
            {
                Error err = new Error
                {
                    modelName = "FirstName",
                    modelErrorMessgae = "First Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate FirstName Length
            if (!Validation.IsNull(buyer.FirstName) && buyer.FirstName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "FirstName",
                    modelErrorMessgae = "First Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate Last Name
            if (Validation.IsNull(buyer.LastName))
            {
                Error err = new Error
                {
                    modelName = "LastName",
                    modelErrorMessgae = "Last Name is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate LastName Length
            if (!Validation.IsNull(buyer.LastName) && buyer.LastName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "LastName",
                    modelErrorMessgae = "Last Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Pass Error Response
            if (!dataValid)
            {
                errors = new ModelError(400, "Pass in Required Information", errorList);
                return BadRequest(errors);
            }


            //create Buyer model
            Buyers newBuyer = new Buyers
            {
                FirstName = buyer.FirstName,
                LastName = buyer.LastName,
                CompanyName = buyer.CompanyName,
                Gender = buyer.Gender,
                Phone = buyer.Phone,
                Email = user.Email,
                MembershipId = 1,
                UserId = userId
            };

            if (!Validation.IsNull(newBuyer.Gender))
            {
                newBuyer.Gender = newBuyer.Gender.ToUpper();
            }


            var result = await _buyersService.AddBuyer(newBuyer);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(userId, newBuyer.Email, "Created Buyer's account");
                _auditLogService.AddAuditLog(auditLog);

                return CreatedAtRoute("GetBuyer", new { UserId = userId }, newBuyer);

            }
            else
            {
                response = new ApiResponse(400, "An error occurred , please try again");
                return BadRequest(response);
            }

        }


        /// <summary>
        /// Update account for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/account/{userId}
        ///     
        ///     {
        ///        FirstName : "John",
        ///        LastName: "Doe",
        ///        CompanyName: "Maxwell Enterprise Inc",
        ///        Phone : "07019000010",
        ///        Gender: "Male"
        ///     }
        ///
        /// </remarks>
        /// <returns>Update Buyer's account</returns>
        /// <response code="200">Account update successful</response>
        /// <response code="400">Pass in required information , create a seller account</response>
        /// <response code="404">User's profile not found</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> Account(int userId, [FromBody] UpdateBuyer buyer)
        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            ApiResponse response;
            bool dataValid = true;

            //get user details
            Users user = await _usersService.GetUser(userId);

            if (user == null)
            {
                response = new ApiResponse(404, "User profile not found");
                return NotFound(response);
            }

            if (user.UserType.ToUpper() == "S")
            {
                response = new ApiResponse(400, "Please, create a seller account");
                return BadRequest(response);
            }


            //Validate First Name
            if (Validation.IsNull(buyer.FirstName))
            {
                Error err = new Error
                {
                    modelName = "FirstName",
                    modelErrorMessgae = "First Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate FirstName Length
            if (!Validation.IsNull(buyer.FirstName) && buyer.FirstName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "FirstName",
                    modelErrorMessgae = "First Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate Last Name
            if (Validation.IsNull(buyer.LastName))
            {
                Error err = new Error
                {
                    modelName = "LastName",
                    modelErrorMessgae = "Last Name is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate LastName Length
            if (!Validation.IsNull(buyer.LastName) && buyer.LastName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "LastName",
                    modelErrorMessgae = "Last Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Pass Error Response
            if (!dataValid)
            {
                errors = new ModelError(400, "Pass in Required Information", errorList);
                return BadRequest(errors);
            }


            //create Buyer model
            Buyers updateBuyer = new Buyers
            {
                FirstName = buyer.FirstName,
                LastName = buyer.LastName,
                CompanyName = buyer.CompanyName,
                Gender = buyer.Gender,
                Phone = buyer.Phone,
            };

            if (!Validation.IsNull(updateBuyer.Gender))
            {
                updateBuyer.Gender = updateBuyer.Gender.ToUpper();
            }


            var result = await _buyersService.UpdateBuyer(userId, updateBuyer);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(userId, user.Email, "Updated Buyer's account");
                _auditLogService.AddAuditLog(auditLog);


                response = new ApiResponse(200, "Account update successful");
                return Ok(response);

            }
            else
            {
                response = new ApiResponse(400, "An error occurred , please try again");
                return BadRequest(response);
            }

        }

    }
}