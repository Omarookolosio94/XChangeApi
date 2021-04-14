using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;
using XChange.Data.Services.Concretes;

namespace XChange.Api.Controllers
{
    [Route("api/address")]
    [ApiController]
    [Produces("application/json")]
    public class AddressController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly IAuditLogService _auditLogService;


        public AddressController()
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
        }


        //GET api/address
        //Get address of all users


        //GET api/address?searchBy=searchWord
        //Search for a given address


        //GET api/address/{userId}
        //Get all address by a user


        //GET api/address/{userId}/{addressId}
        //Get a single address


        //POST api/address/{userID}
        //add address for user


        //PUT api/address/{userID}/{addressId}
        //update a given address
    }
}
