using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XChange.Api.DTO;
using XChange.Api.Models;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;
using XChange.Data.Services.Concretes;

namespace XChange.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly IBuyersService _buyersService;
        private readonly ISellersService _sellersService;
        private readonly IAuditLogService _auditLogService;


        public AccountController()
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _buyersService = new BuyersService(new BuyersRepository(dbContext));
            _sellersService = new SellersService(new SellersRepository(dbContext));
        }


        /// <summary>
        /// Get account based on user type:  Buyers and Sellers
        /// </summary>
        /// <returns>Details of user</returns>
        /// <response code="200">Details of user with hashed password</response>
        /// <response code="404">user not found</response>
        [HttpGet("{userType}", Name = "GetAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> account(string userType)
        {

           // ApiResponse response;
            

            if (userType.ToLower() == "s" || userType.ToLower() == "seller")
            {
                var result = await _sellersService.GetSellers();
                return Ok(result);
            }
            else 
            {
                var result = await _buyersService.GetBuyers();
                return Ok(result);
            }
 
        }

        //POST api/account/{userType}/{userID}
        //Create account for user


        //GET api/account/{userType}/{userID}
        //Get account of a user



        //PUT api/account/{userType}/{userID}
        //update account for user



        //GET api/account/address
        //Get address of all users


        //GET api/account/address?searchBy=searchWord
        //Search for a given address


        //GET api/account/{userID}/address
        //Get all address by a user


        //GET api/account/{userID}/address/{addressID}
        //Get a single address



        //POST api/account/{userID}/address
        //add address for user


        //PUT api/account/{userID}/address?address=addressID
        //update a given address
    }
}