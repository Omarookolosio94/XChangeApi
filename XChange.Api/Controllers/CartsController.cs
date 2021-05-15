using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XChange.Api.DTO;
using XChange.Api.Models;
using XChange.Api.Provider.Concretes;
using XChange.Api.Provider.Interfaces;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Controllers
{
    [Route("api/carts")]
    [ApiController]
    [Produces("application/json")]
    public class CartsController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly ICartsService _cartsService;
        private readonly IProductsService _productsService;
        private readonly IAuditLogService _auditLogService;
        private readonly ICartsProvider _cartsProvider;


        public CartsController()
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _cartsService = new CartsService(new CartsRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _productsService = new ProductsService(new ProductsRepository(dbContext));
            _cartsProvider = new CartsProvider(dbContext);
        }

        /// <summary>
        /// Get all cart item
        /// </summary>
        /// <returns>List of all cart item</returns>
        /// <response code="200">List of all cart item</response>
        /// <response code="500">An error occured, please try again</response>
        [HttpGet(Name = "GetCarts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCarts()
        {
            //var result = _cartsService.GetCarts();
            var result = _cartsProvider.GetCarts();

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
        /// Get Count of carts
        /// </summary>
        /// <returns>Count of carted products</returns>
        /// <response code="200">Count of all carted products</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count", Name = "GetCartCount")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartCount()
        {
            var count = await _cartsService.GetCartCount();
            return Ok(count);
        }

        /// <summary>
        /// Get Count of user's cart
        /// </summary>
        /// <returns>Count of user's carted products</returns>
        /// <response code="200">Count of user's carted products</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{userId}/count", Name = "GetCountOfUserCart")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountOfUserCart(int userId)
        {
            var count = await _cartsService.GetCountOfUserCart(userId);
            return Ok(count);
        }

        /// <summary>
        /// Get users cart
        /// </summary>
        /// <returns>Detail of user's cart</returns>
        /// <response code="200">Details of products in user's cart or no item on cart </response>
        /// <response code="400">An error occured</response>
        [HttpGet("{userId}", Name = "GetUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> GetUserCart(int userId)
        {

            var result = await _cartsService.GetUserCart(userId);
            ApiResponse response;

            if (result != null)
            {
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "No item on cart , add item to cart");
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
        /// Get users cart from auth headers
        /// </summary>
        /// <returns>Detail of user's cart</returns>
        /// <response code="200">Details of products in user's cart or no item on cart </response>
        /// <response code="400">An error occured</response>
        [HttpGet("user-cart",Name = "GetUserCartFromAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> GetUserCartFromAuth()
        {
            ApiResponse response;

            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var result = await _cartsService.GetUserCart(Convert.ToInt32(userId));

            if (result != null)
            {
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "No item on cart , add item to cart");
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
        /// Adds or updates products in user's cart
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Product has been added to user's cart to has been updated in user's cart</response>
        /// <response code="400">An error occurred , please try again</response>
        /// <response code="400">Product does not exist</response>
        [HttpPut("{userId}", Name = "AddItemToCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> AddItemToCart(int userId, [FromQuery] int productId, [FromQuery] int quantity = 1)
        {
            ApiResponse response;
            var userAuthId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            if(Convert.ToInt32(userAuthId) != userId)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            if(quantity < 1)
            {
                response = new ApiResponse(400, "Quantity placed on cart should not be less than 1");
                return BadRequest(response);
            }

            //check if product exits
            var isProduct = await _productsService.IsProduct(productId);

            if (!isProduct)
            {
                response = new ApiResponse(404, "Product does not exist");
                return NotFound(response);
            }

            //Define variables
            Carts cartItem = new Carts
            {
                ProductId = productId,
                UserId = userId,
                QuantityOrdered = quantity
            };


            //check exist in user carts
            var IsProductInUserCart = await _cartsService.IsProductInUserCart(productId, userId);

            if (IsProductInUserCart)
            {
                var result = await _cartsService.UpdateUserCart(userId, cartItem);

                if (result)
                {
                    //add audit log
                    AuditLog auditLog = Utility.Utility.AddAuditLog(userId, "Updated product in cart. Product id: " + productId);
                    _auditLogService.AddAuditLog(auditLog);

                    response = new ApiResponse(200, "Product has been updated in your cart");
                    return Ok(response);

                }
                else
                {
                    response = new ApiResponse(400, "An error occurred , please try again");
                    return BadRequest(response);
                }
            }
            else
            {
                var result = await _cartsService.AddProductToCart(cartItem);

                if (result)
                {
                    //add audit log
                    AuditLog auditLog = Utility.Utility.AddAuditLog(userId, "Added product to cart. Product id: " + productId);
                    _auditLogService.AddAuditLog(auditLog);

                    response = new ApiResponse(200, "Product has been added to your cart");
                    return Ok(response);

                }
                else
                {
                    response = new ApiResponse(400, "An error occurred , please try again");
                    return BadRequest(response);
                }
            }



        }


        /// <summary>
        /// Deletes a user's cart using Auth headers
        /// </summary>
        /// <returns>Delete success message</returns>
        /// <response code="200">Cart has been deleted successfully</response>
        /// <response code="400">Cart does not exist or you are not eligible to carry out this action</response>
        [HttpDelete("user-cart", Name = "EmptyUserCartFromAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> EmptyUserCartFromAuth()
        {
            ApiResponse response;

            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var result = await _cartsService.DeleteUserCart(Convert.ToInt32(userId));

            if (result)
            {
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), "Emptied cart , all products in cart has been removed");
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Cart has been emptied successfully");
                return Ok(response);
            }
            else
            {
                response = new ApiResponse(400, "You do not have a cart.");
                return BadRequest(response);
            }

        }

        /// <summary>
        /// Deletes a user's cart
        /// </summary>
        /// <returns>Delete success message</returns>
        /// <response code="200">Cart has been deleted successfully</response>
        /// <response code="400">You do not have a cart</response>
        [HttpDelete("{userId}", Name = "EmptyUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> EmptyUserCart(int userId)
        {
            ApiResponse response;

            var userAuthId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            if (Convert.ToInt32(userAuthId) != userId)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            var result = await _cartsService.DeleteUserCart(Convert.ToInt32(userId));

            if (result)
            {
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), "Emptied cart , all products in cart has been removed");
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Cart has been emptied successfully");
                return Ok(response);
            }
            else
            {
                response = new ApiResponse(400, "You do not have a cart.");
                return BadRequest(response);
            }

        }


        /// <summary>
        /// Removes a product from user's cart
        /// </summary>
        /// <returns>Delete success message</returns>
        /// <response code="200">Product has been removed from cart successfully</response>
        /// <response code="400">You do not have this product in your cart or You are not eligible to carry out this action</response>
        [HttpDelete("remove-product", Name = "RemoveProductFromUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> RemoveProductFromUserCart([FromQuery] int userId, [FromQuery]int productId)
        {
            ApiResponse response;
            var userAuthId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            if (Convert.ToInt32(userAuthId) != userId)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            var result = await _cartsService.RemoveProductFromCart(productId, userId);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), "Removed product from cart. ProductId: " + productId);
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Product has been removed from cart successfully.");
                return Ok(response);
            }
            else
            {
                response = new ApiResponse(400, "You do not have this product in your cart");
                return BadRequest(response);
            }

        }


    }
}