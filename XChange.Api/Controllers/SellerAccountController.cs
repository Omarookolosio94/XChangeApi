using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/seller-account")]
    [ApiController]
    [Produces("application/json")]
    public class SellerAccountController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly ISellersService _sellersService;
        private readonly IAuditLogService _auditLogService;


        public SellerAccountController()
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _sellersService = new SellersService(new SellersRepository(dbContext));
        }


        /// <summary>
        /// Get seller account of user
        /// </summary>
        /// <returns>Details of Seller</returns>
        /// <response code="200">Details of all sellers</response>
        /// <response code="404">seller not found</response>
        [HttpGet(Name = "GetSellerAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> SellerAccount()
        {
            var result = _sellersService.GetSellers();
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
        /// Search for Sellers
        /// </summary>
        /// <returns>List of Sellers that matches request</returns>
        /// <response code="200">List of Sellers or match not found</response>
        /// <response code="500">An error occured, try again</response>
        [HttpGet("search", Name = "SearchSellers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> SearchAccount([FromQuery] string search)
        {

            var result = await _sellersService.SearchSellers(search.ToString().ToLower());

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
        /// Get account of a Seller
        /// </summary>
        /// <returns>Details of a seller</returns>
        /// <response code="200">Details of a seller</response>
        /// <response code="404">Seller not found</response>
        [HttpGet("{userId}", Name = "GetSeller")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public IActionResult GetSeller(int userId)
        {

            var result = _sellersService.GetSeller(userId);
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
        /// Creates a new seller account for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/seller-account/{userId}
        ///     
        ///     {
        ///        "CompanyName": "BookStores.com",
        ///        "ContactFirtName": "Mariam",
        ///        "ContactLastName": "Benedict",
        ///        "ContactPosition":"Sales Admin",
        ///        "Logo": " ",
        ///        "Phone": " "
        ///     }
        ///
        /// </remarks>
        /// <returns>New seller's account</returns>
        /// <response code="201">Newly created seller's account</response>
        /// <response code="400">Pass in required information , create a seller account, account already exists</response>
        /// <response code="404">User's profile not found</response>
        [HttpPost("{userId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> SellerAccount(int userId, [FromBody] Seller seller)
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

            if (user.UserType.ToUpper() == "B")
            {
                response = new ApiResponse(400, "Please, create a buyer's account");
                return BadRequest(response);
            }

            var isRegistered = await _sellersService.IsSellerRegistered(userId);

            //check if user has an account already
            if (isRegistered)
            {
                response = new ApiResponse(400, "Account already exist");
                return BadRequest(response);
            }

            //Validate Company Name
            if (Validation.IsNull(seller.CompanyName))
            {
                Error err = new Error
                {
                    modelName = "CompanyName",
                    modelErrorMessgae = "Company Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate CompanyName Length
            if (!Validation.IsNull(seller.CompanyName) && seller.CompanyName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "CompanyName",
                    modelErrorMessgae = "Company Name  should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate Contact First Name
            if (Validation.IsNull(seller.ContactFirstName))
            {
                Error err = new Error
                {
                    modelName = "ContactFirstName",
                    modelErrorMessgae = "Contact First Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate ContactFirstName Length
            if (!Validation.IsNull(seller.ContactFirstName) && seller.ContactFirstName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "ContactFirstName",
                    modelErrorMessgae = "Contact First Name  should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate Contact Last Name
            if (Validation.IsNull(seller.ContactLastName))
            {
                Error err = new Error
                {
                    modelName = "ContactLastName",
                    modelErrorMessgae = "Contact Last Name is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate ContactLastName Length
            if (!Validation.IsNull(seller.ContactLastName) && seller.ContactLastName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "ContactLastName",
                    modelErrorMessgae = "Contact Last Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }


            //Validate Phone Number
            if (Validation.IsNull(seller.Phone))
            {
                Error err = new Error
                {
                    modelName = "Phone",
                    modelErrorMessgae = "Phone Number is Required",
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
            Sellers newSeller = new Sellers
            {
                CompanyName = seller.CompanyName,
                ContactFirstName = seller.ContactFirstName,
                ContactLastName = seller.ContactLastName,
                ContactPosition = seller.ContactPosition,
                Logo = seller.Logo,
                Phone = seller.Phone,
                Email = user.Email,
                MembershipId = 2,
                UserId = userId
            };


            var result = await _sellersService.AddSeller(newSeller);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(userId, newSeller.Email, "Created Seller's account");
                _auditLogService.AddAuditLog(auditLog);

                return CreatedAtRoute("GetSeller", new { UserId = userId }, newSeller);

            }
            else
            {
                response = new ApiResponse(400, "An error occurred , please try again");
                return BadRequest(response);
            }

        }


        /// <summary>
        /// Update seller-account for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/seller-account/{userId}
        ///     
        ///     {
        ///        "CompanyName": "BookStores.com",
        ///        "ContactFirtName": "Mariam",
        ///        "ContactLastName": "Benedict",
        ///        "ContactPosition":"Sales Admin",
        ///        "Logo": " ",
        ///        "Phone": " "
        ///     }
        ///
        /// </remarks>
        /// <returns>Update Seller's account</returns>
        /// <response code="200">Seller Account update successful</response>
        /// <response code="400">Pass in required information , create a Buyer account</response>
        /// <response code="404">User's profile not found</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> SellerAccount(int userId, [FromBody] UpdateSeller seller)
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

            if (user.UserType.ToUpper() == "B")
            {
                response = new ApiResponse(400, "Please, create a buyer's account");
                return BadRequest(response);
            }

             
          
            //Validate Company Name
            if (Validation.IsNull(seller.CompanyName))
            {
                Error err = new Error
                {
                    modelName = "CompanyName",
                    modelErrorMessgae = "Company Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate CompanyName Length
            if (!Validation.IsNull(seller.CompanyName) && seller.CompanyName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "CompanyName",
                    modelErrorMessgae = "Company Name  should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate Contact First Name
            if (Validation.IsNull(seller.ContactFirstName))
            {
                Error err = new Error
                {
                    modelName = "ContactFirstName",
                    modelErrorMessgae = "Contact First Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate ContactFirstName Length
            if (!Validation.IsNull(seller.ContactFirstName) && seller.ContactFirstName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "ContactFirstName",
                    modelErrorMessgae = "Contact First Name  should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate Contact Last Name
            if (Validation.IsNull(seller.ContactLastName))
            {
                Error err = new Error
                {
                    modelName = "ContactLastName",
                    modelErrorMessgae = "Contact Last Name is Required",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //Validate ContactLastName Length
            if (!Validation.IsNull(seller.ContactLastName) && seller.ContactLastName.Length > 40)
            {
                Error err = new Error
                {
                    modelName = "ContactLastName",
                    modelErrorMessgae = "Contact Last Name should be less than or equal to 40",
                };

                errorList.Add(err);
                dataValid = false;

            }


            //Validate Phone Number
            if (Validation.IsNull(seller.Phone))
            {
                Error err = new Error
                {
                    modelName = "Phone",
                    modelErrorMessgae = "Phone Number is Required",
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
            Sellers updateSeller = new Sellers
            {
                CompanyName = seller.CompanyName,
                ContactFirstName = seller.ContactFirstName,
                ContactLastName = seller.ContactLastName,
                ContactPosition = seller.ContactPosition,
                Logo = seller.Logo,
                Phone = seller.Phone,
            };


            var result = await _sellersService.UpdateSeller(userId, updateSeller);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(userId, user.Email, "Updated Seller's account");
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