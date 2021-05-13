using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XChange.Api.DTO;
using XChange.Api.Models;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IProductsService _productsService;
        private readonly IUsersService _usersService;
        private readonly ISellersService _sellersService;
        private readonly IAuditLogService _auditLogService;
        private readonly IOrdersService _ordersService;

        public OrdersController()
        {
            _productsService = new ProductsService(new ProductsRepository(dbContext));
            _sellersService = new SellersService(new SellersRepository(dbContext));
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _ordersService = new OrdersService(new OrdersRepository(dbContext));
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>List of all orders</returns>
        /// <response code="200">List of all orders</response>
        /// <response code="400">An error occured, please try again</response>
        [HttpGet(Name = "GetOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _ordersService.GetOrders();
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(400, "An error ocurred, please try again");
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get all user's orders
        /// </summary>
        /// <returns>List of all orders by user</returns>
        /// <response code="200">List of all orders by user or no order has been placed</response>
        /// <response code="400">An error occured, please try again</response>
        [HttpGet("{userId}", Name = "GetUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var result = await _ordersService.GetUserOrders(userId);
            ApiResponse response;

            if (result != null)
            {
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "No order has been placed.To place an order add item to cart and checkout");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(400, "An error occured, please try again");
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get all user's valid orders
        /// </summary>
        /// <returns>List of all valid or active orders by user</returns>
        /// <response code="200">List of all valid orders by user or all order has been closed/response>
        /// <response code="400">An error occured, please try again</response>
        [HttpGet("{userId}/valid-orders", Name = "GetUserValidOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserValidOrders(int userId)
        {
            var result = await _ordersService.GetUserValidOrders(userId);
            ApiResponse response;

            if (result != null)
            {
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "All order has been closed.To place an order add item to cart and checkout");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(400, "An error occured, please try again");
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get a single user order
        /// </summary>
        /// <returns>Retrieves single user order</returns>
        /// <response code="200">Details of single user order</response>
        /// <response code="404">Order not found, please try again</response>
        [HttpGet("{userId}/{orderId}", Name = "GetSingleUserOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSingleUserOrder(int userId, int orderId)
        {
            var result = await _ordersService.GetSingleUserOrder(orderId, userId);
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(404, "Order not found , please try again");
                return NotFound(response);
            }
        }


        /// <summary>
        /// Get Count of all orders
        /// </summary>
        /// <returns>Count of all orderss</returns>
        /// <response code="200">Count of all orders</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count", Name = "GetOrdersCount")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrdersCount()
        {
            var count = await _ordersService.GetOrdersCount();
            return Ok(count);
        }


        /// <summary>
        /// Get Count of all user's orders
        /// </summary>
        /// <returns>Count of all user's orders</returns>
        /// <response code="200">Count of all user's orders</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{userId}/count", Name = "GetUserOrdersCount")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserOrdersCount(int userId)
        {
            var count = await _ordersService.GetUserOrdersCount(userId);
            return Ok(count);
        }

        /// <summary>
        /// Get Count of all user's valid orders
        /// </summary>
        /// <returns>Count of all user's valid orders</returns>
        /// <response code="200">Count of all user's valid orders</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{userId}/valid-order/count", Name = "GetUserValidOrdersCount")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserValidOrdersCount(int userId)
        {
            var count = await _ordersService.GetUserValidOrdersCount(userId);
            return Ok(count);
        }


        /// <summary>
        /// Query all orders using order-status
        /// </summary>
        /// <remarks>
        /// query order using the following parameters
        /// 
        ///         Pending
        ///         Processing
        ///         Verified
        ///         Completed
        ///         Cancelled
        ///         
        /// </remarks>
        /// <returns>List of all orders matching query</returns>
        /// <response code="200">List of all orders that match query string or no match found</response>
        /// <response code="400">Query not valid or an error occured, please try again</response>
        [HttpGet("search", Name = "QueryOrderByOrderStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> QueryOrderByOrderStatus([FromQuery]string query)
        {
            ApiResponse response;
            var queryString = query.ToLower();

            if (queryString != "pending" && queryString != "processing" && queryString != "verified" && queryString != "completed" && queryString != "cancelled")
            {
                response = new ApiResponse(400, "Query not valid , search for either Pending , Processing , Verified , Completed or Cancelled.");
                return BadRequest(response);
            }

            var result = await _ordersService.QueryOrderByOrderStatus(queryString);

            if (result != null)
            {
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "No order matches search. To place an order add item to cart and checkout");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(400, "An error ocurred, please try again");
                return BadRequest(response);
            }
        }


        /// <summary>
        /// Query all user's order using order-status
        /// </summary>
        /// <remarks>
        /// query user order using the following parameters
        /// 
        ///         Pending
        ///         Processing
        ///         Verified
        ///         Completed
        ///         Cancelled
        ///         
        /// </remarks>
        /// <returns>List of all user's orders matching query</returns>
        /// <response code="200">List of all user's orders that match query string or no match found</response>
        /// <response code="400">Query not valid or an error occured, please try again</response>
        [HttpGet("{userId}/search", Name = "QueryUserOrderByOrderStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> QueryUserOrderByOrderStatus(int userId ,[FromQuery]string query)
        {
            ApiResponse response;
            var queryString = query.ToLower();

            if (queryString != "pending" && queryString != "processing" && queryString != "verified" && queryString != "completed" && queryString != "cancelled")
            {
                response = new ApiResponse(400, "Query not valid , search for either Pending , Processing , Verified , Completed or Cancelled.");
                return BadRequest(response);
            }

            var result = await _ordersService.QueryUserOrderByOrderStatus(userId, queryString);

            if (result != null)
            {
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "No order matches search. To place an order add item to cart and checkout");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(400, "An error ocurred, please try again");
                return BadRequest(response);
            }
        }


        //POST place an order

        //PUT cancel an order




    }
}