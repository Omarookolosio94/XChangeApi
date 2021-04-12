using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XChange.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        //GET api/account?account=userType
        //Get account based on user type:  Buyers and Sellers



        //GET api/account/{userID}
        //Get account of a user



        //POST api/account/{userID}
        //Create account for user


        //PUT api/account/{userID}
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