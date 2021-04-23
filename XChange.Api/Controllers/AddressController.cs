using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/address")]
    [ApiController]
    [Produces("application/json")]
    public class AddressController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly IAuditLogService _auditLogService;
        private readonly IAddressService _addressService;


        public AddressController()
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _addressService = new AddressService(new AddressRepository(dbContext));
        }


        /// <summary>
        /// Get address of all users
        /// </summary>
        /// <returns>List of users address</returns>
        /// <response code="200">Address of users</response>
        /// <response code="500">An error occured, please try again</response>
        [HttpGet(Name = "GetAllAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Address()
        {
            var result = await _addressService.GetAllAddress();
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(500, "An error ocurred, please try again");
                return NotFound(response);
            }
        }


        /// <summary>
        /// Search for a given address
        /// </summary>
        /// <returns>List of address that matches request</returns>
        /// <response code="200">List of address or match not found</response>
        /// <response code="500">An error occurred , try again</response>
        [HttpGet("search", Name = "SearchAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> SearchAddress([FromQuery] string search)
        {

            var result = await _addressService.SearchAddress(search.ToString().ToLower());

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
        /// Get single address
        /// </summary>
        /// <returns>A single address</returns>
        /// <response code="200">Single address</response>
        /// <response code="404">User not found</response>
        [HttpGet("{addressId}", Name = "GetSingleAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetSingleAddress(int addressId)
        {

            var result = await _addressService.GetSingleAddress(addressId);
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(404, "Address not found, Please Create one");
                return NotFound(response);
            }

        }


        /// <summary>
        /// Get all address of a user
        /// </summary>
        /// <returns>List of a user address</returns>
        /// <response code="200">Address of a user</response>
        /// <response code="500">An error occured, please try again</response>
        [HttpGet("{userId}/addresses", Name = "GetAddressOfUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public IActionResult GetAddressOfUser(int userId)
        {

            var result = _addressService.GetAddressOfUser(userId);
            ApiResponse response;

            if (result != null)
            {
                return Ok(result.Result);
            }
            else
            {
                response = new ApiResponse(500, "An error occurred , please try again");
                return NotFound(response);
            }

        }



        /// <summary>
        /// Get single address of a user
        /// </summary>
        /// <returns>A single address</returns>
        /// <response code="200">Single address of a user</response>
        /// <response code="404">Address not found</response>
        [HttpGet("{userId}/{addressId}", Name = "GetSingleAddressOfUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetSingleAddressOfUser(int userId, int addressId)
        {

            var result = await _addressService.GetSingleAddressOfUser(userId, addressId);
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(404, "Address not found, Please Create one");
                return NotFound(response);
            }

        }


        /// <summary>
        /// Add address for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/address/{userId}
        ///     
        ///     {
        ///        "AddressType" : " ",
        ///        "Street": "48 Vaughan Steet",
        ///        "City": "Ebute Metta",
        ///        "State": "Lagos",
        ///        "Country": "Nigeria",
        ///        "PostalCode": ""
        ///     }
        ///
        /// </remarks>
        /// <returns>Address created success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="400">Pass in required information</response>
        /// <response code="404">User's profile not found</response>
        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> Address(int userId, [FromBody] AddressDTO address)
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


            //Validate Street
            if (Validation.IsNull(address.Street))
            {
                Error err = new Error
                {
                    modelName = "Street",
                    modelErrorMessgae = "Street is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate Street Length
            if (!Validation.IsNull(address.Street) && address.Street.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "Street",
                    modelErrorMessgae = "Street should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate City
            if (Validation.IsNull(address.City))
            {
                Error err = new Error
                {
                    modelName = "City",
                    modelErrorMessgae = "City is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate City Length
            if (!Validation.IsNull(address.City) && address.City.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "City",
                    modelErrorMessgae = "City should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate State
            if (Validation.IsNull(address.State))
            {
                Error err = new Error
                {
                    modelName = "State",
                    modelErrorMessgae = "State is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate State Length
            if (!Validation.IsNull(address.State) && address.State.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "State",
                    modelErrorMessgae = "State should be less than or equal to 40",
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


            //create Address model
            Address newAddress = new Address
            {
                AdressType = address.AdressType,
                City = address.City,
                Street = address.Street,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,
                UserId = userId
            };


            var result = await _addressService.AddAddress(newAddress);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(userId, user.Email, "Added address: " + newAddress.Street );
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Address added successfully");
                return Ok(response);
            }
            else
            {
                response = new ApiResponse(400, "An error occurred , please try again");
                return BadRequest(response);
            }

        }

        //PUT api/address/{userID}/{addressId}
        //update a given address

        /// <summary>
        /// Update address for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/address/{userId}/{addressId}
        ///     {
        ///        "AddressType" : " ",
        ///        "Street": "48 Vaughan Steet",
        ///        "City": "Ebute Metta",
        ///        "State": "Lagos",
        ///        "Country": "Nigeria",
        ///        "PostalCode": ""
        ///     }
        ///
        /// </remarks>
        /// <returns>Address update success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="400">Pass in required information</response>
        /// <response code="404">User's profile not found</response>
        [HttpPut("{userId}/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> Address(int userId,int addressId, [FromBody] AddressDTO address)
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
 
            //Validate Street
            if (Validation.IsNull(address.Street))
            {
                Error err = new Error
                {
                    modelName = "Street",
                    modelErrorMessgae = "Street is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate Street Length
            if (!Validation.IsNull(address.Street) && address.Street.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "Street",
                    modelErrorMessgae = "Street should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate City
            if (Validation.IsNull(address.City))
            {
                Error err = new Error
                {
                    modelName = "City",
                    modelErrorMessgae = "City is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate City Length
            if (!Validation.IsNull(address.City) && address.City.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "City",
                    modelErrorMessgae = "City should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate State
            if (Validation.IsNull(address.State))
            {
                Error err = new Error
                {
                    modelName = "State",
                    modelErrorMessgae = "State is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate State Length
            if (!Validation.IsNull(address.State) && address.State.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "State",
                    modelErrorMessgae = "State should be less than or equal to 40",
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


            //create Address model
            Address updateAddress = new Address
            {
                AddressId = addressId,
                AdressType = address.AdressType,
                City = address.City,
                Street = address.Street,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,
            };


            var result = await _addressService.UpdateAddress(userId, updateAddress);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(userId, user.Email, "Updated address: " + updateAddress.Street);
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Address updated successfully");
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
