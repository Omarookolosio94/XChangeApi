using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Validation;
using XChange.Api.DTO;
using XChange.Api.Models;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Services.Concretes;
using XChange.Api.Services.Interfaces;
using XChange.Api.Utility;
using XChange.Api.Utility.Pdf_Utility;
using static XChange.Api.DTO.ModelError;

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
        private readonly ICartsService _cartsService;
        private readonly IAddressService _addressService;
        private readonly IConverter _converter;


        public OrdersController(IConverter converter)
        {
            _productsService = new ProductsService(new ProductsRepository(dbContext));
            _sellersService = new SellersService(new SellersRepository(dbContext));
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _ordersService = new OrdersService(new OrdersRepository(dbContext));
            _cartsService = new CartsService(new CartsRepository(dbContext));
            _addressService = new AddressService(new AddressRepository(dbContext));
            _converter = converter;
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
        /// <response code="200">List of all valid orders by user or all order has been close</response>
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
        public async Task<IActionResult> QueryUserOrderByOrderStatus(int userId, [FromQuery]string query)
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



        /// <summary>
        /// Make an order
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/orders/{userId}
        ///     
        ///     {
        ///         "Billing_Address": "18 Agadage Avenue ,Ekpan",
        ///         "Billing_Phone": "07019068486",
        ///         "Summary":"Making purchase",
        ///         "Tag":"Bracelets , Chains",
        ///         "UseSavedAddress": false
        ///     }
        ///
        /// </remarks>
        /// <returns>Details of receipts</returns>
        /// <response code="201">Returns Receipt showing total price and details of all products ordered</response>
        /// <response code="400">Product no longer in store or pass in required information or order failed, please try again</response>
        [HttpPost("{userId}", Name = "MakeOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> MakeOrder(int userId , Order order)
        {
            ApiResponse response;
            ModelError errors;
            Reciept reciept = new Reciept {};
            List<OrderedProducts> orderedProductsList = new List<OrderedProducts> { };
            List<Error> errorList = new List<Error> { };

            bool dataValid = true;
            string saved_Address = "";
            int saved_Address_Id = 0;
            string productsId = "";

            //Fetch cart items
            var cartItems = await _cartsService.GetUserCart(userId);

            decimal total_price = 0;
            decimal total_tax = 0;
            int total_weight = 0;

            foreach (Carts item in cartItems)
            {
                //fetch product and calculate price
                var isProduct = await _productsService.IsProduct(item.ProductId);

                if (isProduct)
                {
                    var productPrice = await _productsService.GetProductPrice(item.ProductId);
                    var price = productPrice * item.QuantityOrdered;

                    OrderedProducts orderedProducts = new OrderedProducts
                    {
                        Product_Id = item.ProductId,
                        Quantity_Ordered = item.QuantityOrdered,
                        Unit_Price = productPrice,
                        Price = price
                    };

                    orderedProductsList.Add(orderedProducts);
                    total_price = total_price + price;
                    productsId = productsId + item.ProductId.ToString() + ",";
                }
                else
                {
                    response = new ApiResponse(400, "Product with productId: " + item.ProductId + " is no longer available in store. Please remove item from cart and proceed with your order.");
                    return BadRequest(response);
                }
            }

            //validate Biiling phone number
            if (Validation.IsNull(order.Billing_Phone))
            {
                Error err = new Error
                {
                    modelName = "Billing_Phone",
                    modelErrorMessgae = "Phone Number is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //validate billing phone
            if (!Validation.IsNull(order.Billing_Phone) && (order.Billing_Phone.Length != 11 && order.Billing_Phone.Length != 13) )
            {
                Error err = new Error
                {
                    modelName = "Billing_Phone",
                    modelErrorMessgae = "Phone Number should be 11 or 13 digits",
                };

                errorList.Add(err);
                dataValid = false;
            }


            if (!order.UseSavedAddress)
            {
                //validate Biiling address
                if (Validation.IsNull(order.Billing_Address))
                {
                    Error err = new Error
                    {
                        modelName = "Billing_Address",
                        modelErrorMessgae = "Address is Required",
                    };

                    errorList.Add(err);
                    dataValid = false;
                }
            } else
            {
                var addressList = await _addressService.GetAddressOfUser(userId);

                if (addressList.Count > 0)
                {
                    saved_Address = addressList[0].Street + ", " + addressList[0].City + ", " + addressList[0].State;
                    saved_Address_Id = addressList[0].AddressId;

                } else
                {
                    Error err = new Error
                    {
                        modelName = "UseSavedAddress",
                        modelErrorMessgae = "You have no saved address, please input billing address",
                    };

                    errorList.Add(err);
                    dataValid = false;

                }
            }
         

            if (!dataValid)
            {
                errors = new ModelError(400, "Pass in Required Information", errorList);
                return BadRequest(errors);
            }


            //get user IP address
            //get shipperId and address
            //var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;

            //Build Order
            Orders makeOrder = new Orders
            {
                UserId = userId,
                BillingAddressId = saved_Address_Id,
                BillingAddress = order.UseSavedAddress ? saved_Address : order.Billing_Address,
                BillingPhone = order.Billing_Phone,
                Currency = "NGN",
                CreatedAt = DateTime.Now,
                OrderStatus = "Pending",
                Summary = order.Summary,
                Tag = order.Tag,
                Source = "Web",
                IpAddress = " ",

                //ShipperId = 0,
                //ShippingAddressId = 0,

                PaymentStatus = "pending",
                ProductsId = productsId,
                SubtotalPrice = total_price,
                TotalTax = total_tax,
                TotalPrice = total_price + total_tax,
                TotalWeight = total_weight,
            };

            var result = await _ordersService.MakeOrder(makeOrder);

            if (result)
            {
                //generate receipt
                reciept.Order_Id = makeOrder.OrderId;
                reciept.Order_Status = "Pending";
                reciept.Total_Price = total_price + total_tax;
                reciept.User_Id = userId;
                reciept.OrderedProducts = orderedProductsList;
                reciept.Billing_Address = makeOrder.BillingAddress;
                reciept.Billing_Phone = makeOrder.BillingPhone;


                //generate receipt pdf
                var receiptHTMLTemplate = PDF_Template_Generator.Get_Orders_Receipt_HTML_Template(reciept);

                var pdfFile = PDF_Utility.Create_Order_PDF(makeOrder.OrderId, userId, receiptHTMLTemplate);
                _converter.Convert(pdfFile);


                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(userId, "Made an order for the following products: " + productsId +  " Total_Price: " + reciept.Total_Price + " Order_Id: " + makeOrder.OrderId + "Reciept: " + JsonSerializer.Serialize(reciept));
                _auditLogService.AddAuditLog(auditLog);

                return Ok(reciept);
            }
            else
            {
                response = new ApiResponse(400, "Order failed , please place another");
                return BadRequest(response);
            }
        }

        //PUT cancel an order


    }
}


//TODO:

// Add order's error log to view errors related to orders
// Save receipt and send copy to user email for verification
// save link of receipt in database
// delete items from cart after order has been placed successfully.
