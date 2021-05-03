using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IProductsService _productsService;
        private readonly IUsersService _usersService;
        private readonly ISellersService _sellersService;
        private readonly IAuditLogService _auditLogService;
        private readonly IGoogleCloudStorageService _googleCloudStorageService;

        public ProductsController(IGoogleCloudStorageService googleCloudStorageService)
        {
            _productsService = new ProductsService(new ProductsRepository(dbContext));
            _sellersService = new SellersService(new SellersRepository(dbContext));
            _usersService = new UsersService(new UsersRepository(dbContext));
            _googleCloudStorageService = googleCloudStorageService;
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
        }

        /// <summary>
        /// Get list of all products
        /// </summary>
        /// <returns>Details of all products</returns>
        /// <response code="200">Details of all products</response>
        /// <response code="500">An error occured, please try again</response>
        [HttpGet(Name = "GetProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Products()
        {
            var result = await _productsService.GetProducts();
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
        /// Get Count of all products
        /// </summary>
        /// <returns>Count of all products</returns>
        /// <response code="200">Count of all products</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count", Name = "GetProductsCount")]
        [AllowAnonymous]
        public async Task<IActionResult> Count()
        {
            var count = await _productsService.GetProductsCount();
            return Ok(count);
        }


        /// <summary>
        /// Search for Products
        /// </summary>
        /// <returns>List of Products that matches request</returns>
        /// <response code="200">List of products or match not found</response>
        /// <response code="500">An error occurred , try again</response>
        [HttpGet("search", Name = "SearchProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProducts([FromQuery] string search)
        {

            var result = await _productsService.SearchProducts(search.ToLower());

            ApiResponse response;

            if (result != null)
            {
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "product match not found");
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
        /// Get products of a seller
        /// </summary>
        /// <returns>Details of products by a given seller</returns>
        /// <response code="200">Details of all products by a seller or no product has been listed by seller</response>
        /// <response code="404">An error occurred , please try again</response>
        [HttpGet("seller/{sellerId}", Name = "GetProductBySeller")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductBySeller(int sellerId)
        {

            var result = await _productsService.GetProductsBySeller(sellerId);
            ApiResponse response;

            if (result != null)
            {
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "No products has been listed by seller");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(404, "an error occurred , try again");
                return NotFound(response);
            }

        }


        /// <summary>
        /// Get single product
        /// </summary>
        /// <returns>Details of product</returns>
        /// <response code="200">Details of product</response>
        /// <response code="404">product not found</response>
        [HttpGet("{productId}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct(int productId)
        {

            var result = await _productsService.GetProduct(productId);
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(404, "product not found");
                return NotFound(response);
            }

        }


        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/products
        ///     
        ///     {
        ///        "Category":"",
        ///        "ProductName": "",
        ///        "Quantity": 1,
        ///        "UnitPrice": 200,
        ///        "ProductDescription": "",
        ///        "UnitsInStock": 10,
        ///        "UnitsInOrder": 0,
        ///     }
        ///
        /// </remarks>
        /// <returns>New  product</returns>
        /// <response code="201">Newly created product</response>
        /// <response code="400">Pass in required information , You are not eligible to carry out this action</response>
        [HttpPost(Name ="AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "S")]
        public async Task<IActionResult> Products([FromForm] Product product)
        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            ApiResponse response;
            bool dataValid = true;

            //Get user Id from token
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            //get sellers details
            var seller = await _sellersService.GetSeller(Convert.ToInt32(userId));

            if (seller == null)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            //validate ProductName
            if (Validation.IsNull(product.ProductName))
            {
                Error err = new Error
                {
                    modelName = "ProductName",
                    modelErrorMessgae = "Product Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate ProductName Length
            if (!Validation.IsNull(product.ProductName) && product.ProductName.Length > 45)
            {
                Error err = new Error
                {
                    modelName = "ProductName",
                    modelErrorMessgae = "Product Name should be less than or equal to 45",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //validate ProductCategory
            if (Validation.IsNull(product.Category))
            {
                Error err = new Error
                {
                    modelName = "ProductCategory",
                    modelErrorMessgae = "Product Category is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate ProductCategory Length
            if (!Validation.IsNull(product.Category) && product.Category.Length > 45)
            {
                Error err = new Error
                {
                    modelName = "ProductCategory",
                    modelErrorMessgae = "Product Category should be less than or equal to 45",
                };

                errorList.Add(err);
                dataValid = false;
            }


            //validate ProductDescription
            if (Validation.IsNull(product.ProductDescription))
            {
                Error err = new Error
                {
                    modelName = "ProductDescription",
                    modelErrorMessgae = "Product Description is Required",
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


            //create Product Model
            Products newProduct = new Products
            {
                ProductName = product.ProductName,
                Category = product.Category,
                ProductDescription = product.ProductDescription,
                TimeAdded = DateTime.Now,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                UnitsInOrder = product.UnitsInOrder,
                UnitsInStock = product.UnitsInStock,
                SellerId = seller.SellerId,
                LastUpdateTime = DateTime.Now,
                Rating = "0"
            };

            //upload image to cloud and assign value to products
            if (product.Picture != null)
            {
                var image = await UploadFile(product);

                newProduct.PictureName = image.PictureName;
                newProduct.PictureUrl = image.PictureUrl;
            }


            var result = await _productsService.AddProduct(newProduct);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), seller.Email, "Added a new product: " +newProduct.ProductName);
                _auditLogService.AddAuditLog(auditLog);

                return CreatedAtRoute("GetProduct", new { ProductId = newProduct.ProductId }, newProduct);
            }
            else
            {
                response = new ApiResponse(400, "An error occurred , please try again");
                return BadRequest(response);
            }

        }

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/products
        ///     
        ///     {
        ///        "Category":"",
        ///        "ProductName": "",
        ///        "Quantity": 1,
        ///        "UnitPrice": 200,
        ///        "ProductDescription": "",
        ///        "UnitsInStock": 10,
        ///        "UnitsInOrder": 0,
        ///     }
        ///
        /// </remarks>
        /// <returns>Updated product success message</returns>
        /// <response code="200">Product update successful</response>
        /// <response code="400">Pass in required information , product not found or you are not eligible to carry out this action , please try again</response>
        [HttpPut("{productId}" ,Name ="UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "S")]
        public async Task<IActionResult> Products(int productId , [FromForm] Product product)
        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            ApiResponse response;
            bool dataValid = true;

            //Get user Id from token
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            //get sellers details
            var seller = await _sellersService.GetSeller(Convert.ToInt32(userId));

            //product check
            var checkProduct = await _productsService.GetProduct(productId);

            if(checkProduct == null)
            {
                response = new ApiResponse(400, "Product was not found");
                return BadRequest(response);
            }

            if (seller == null)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            if(checkProduct.SellerId != seller.SellerId)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            //validate ProductName
            if (Validation.IsNull(product.ProductName))
            {
                Error err = new Error
                {
                    modelName = "ProductName",
                    modelErrorMessgae = "Product Name is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate ProductName Length
            if (!Validation.IsNull(product.ProductName) && product.ProductName.Length > 45)
            {
                Error err = new Error
                {
                    modelName = "ProductName",
                    modelErrorMessgae = "Product Name should be less than or equal to 45",
                };

                errorList.Add(err);
                dataValid = false;

            }

            //validate ProductCategory
            if (Validation.IsNull(product.Category))
            {
                Error err = new Error
                {
                    modelName = "ProductCategory",
                    modelErrorMessgae = "Product Category is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate ProductCategory Length
            if (!Validation.IsNull(product.Category) && product.Category.Length > 45)
            {
                Error err = new Error
                {
                    modelName = "ProductCategory",
                    modelErrorMessgae = "Product Category should be less than or equal to 45",
                };

                errorList.Add(err);
                dataValid = false;
            }


            //validate ProductDescription
            if (Validation.IsNull(product.ProductDescription))
            {
                Error err = new Error
                {
                    modelName = "ProductDescription",
                    modelErrorMessgae = "Product Description is Required",
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

            //create Product Model
            Products updateProduct = new Products
            {
                ProductName = product.ProductName,
                Category = product.Category,
                ProductDescription = product.ProductDescription,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                UnitsInOrder = product.UnitsInOrder,
                UnitsInStock = product.UnitsInStock,
                ProductId = productId
            };

            //upload image to cloud and assign value to products
            if (product.Picture != null)
            {
                //Delete Previous Image from google cloud
                if (checkProduct.PictureName != null)
                {
                    await _googleCloudStorageService.DeleteFileAsync(checkProduct.PictureName);
                }

                var image = await UploadFile(product);
                updateProduct.PictureUrl = image.PictureUrl;
                updateProduct.PictureName = image.PictureName;

            }


            var result = await _productsService.UpdateProduct(seller.SellerId , updateProduct);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), seller.Email, "Updated product: " + updateProduct.ProductName);
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Product update successful");
                return Ok(response);
            }
            else
            {
                //Delete Upload File from google storage
                if (product.Picture != null)
                {
                    if (updateProduct.PictureName != null)
                    {
                        await _googleCloudStorageService.DeleteFileAsync(updateProduct.PictureName);
                    }
                }

                response = new ApiResponse(400, "Product update failed , please try again");
                return BadRequest(response);
            }

        }


        /// <summary>
        /// Updates a products picture
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/products/{productId}/image
        ///     {
        ///        "Image": file
        ///     }
        ///
        /// </remarks>
        /// <returns>Updated product image success message</returns>
        /// <response code="200">Product picture update successful</response>
        /// <response code="400">Pass in required information , product not found , you are not eligible to carry out this action or please try again</response>
        [HttpPut("{productId}/image", Name = "UploadProductPicuture")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "S")]
        public async Task<IActionResult> UploadProductPicture(int productId, [FromForm] ImageUpload image)
        {
            ApiResponse response;

            //Get user Id from token
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            //get sellers details
            var seller = await _sellersService.GetSeller(Convert.ToInt32(userId));

            //product check
            var checkProduct = await _productsService.GetProduct(productId);

            if (checkProduct == null)
            {
                response = new ApiResponse(400, "Product was not found");
                return BadRequest(response);
            }

            if (seller == null)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            if (checkProduct.SellerId != seller.SellerId)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }
 
 
            //upload image to cloud and assign value to products
            if (image.Image != null)
            {
                //Delete Previous Image from google cloud
                if (checkProduct.PictureName != null)
                {
                    await _googleCloudStorageService.DeleteFileAsync(checkProduct.PictureName);
                }

                var updateImage = await UploadFile(checkProduct.ProductName , image.Image);

                checkProduct.PictureUrl = updateImage.PictureUrl;
                checkProduct.PictureName = updateImage.PictureName;
            }


            var result = await _productsService.UpdateProduct(seller.SellerId, checkProduct);

            if (result)
            {

                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), seller.Email, "Added product picture: " + checkProduct.ProductName);
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Product picture update successful");
                return Ok(response);
            }
            else
            {
                //Delete Upload File from google storage
                if (image.Image != null)
                {
                    if (checkProduct.PictureName != null)
                    {
                        await _googleCloudStorageService.DeleteFileAsync(checkProduct.PictureName);
                    }
                }

                response = new ApiResponse(400, "Product picture update failed , try again");
                return BadRequest(response);
            }

        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <returns>Delete success message</returns>
        /// <response code="200">Product has been deleted successfully</response>
        /// <response code="400">Product does not exist or you are not eligible to carry out this action</response>
        [HttpDelete("{productId}" , Name ="DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "S")]
        public async Task<IActionResult> Products(int productId)
        {
            ApiResponse response;

            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var seller = await _sellersService.GetSeller(Convert.ToInt32(userId));

            //get product
            var checkProduct = await _productsService.GetProduct(productId);

            if(checkProduct == null)
            {
                response = new ApiResponse(400, "Product does not exist");
                return BadRequest(response);
            }

            if (seller == null)
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            var result = await _productsService.DeleteProduct(seller.SellerId, productId);

            if (result)
            {
                //Delete Image from google cloud

                if (checkProduct.PictureName != null)
                {
                    await _googleCloudStorageService.DeleteFileAsync(checkProduct.PictureName);
                }

                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), seller.Email, "Deleted product: " + productId);
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Product has been deleted successfully");
                return Ok(response);
            }
            else
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

        }

        private async Task<PictureResponse> UploadFile(Product product)
        {
            string imageNameForStorage = FormFileName(product.ProductName, product.Picture.FileName);

            var imageUrl = await _googleCloudStorageService.UploadFileAsync(product.Picture, imageNameForStorage);
            var imageName = imageNameForStorage;

            PictureResponse response = new PictureResponse
            {
                PictureName = imageName,
                PictureUrl = imageUrl
            };


            return response;
        }

        private async Task<PictureResponse> UploadFile(string productName, IFormFile file )
        {
            string imageNameForStorage = FormFileName(productName, file.FileName);

            var imageUrl = await _googleCloudStorageService.UploadFileAsync(file, imageNameForStorage);
            var imageName = imageNameForStorage;

            PictureResponse response = new PictureResponse
            {
                PictureName = imageName,
                PictureUrl = imageUrl
            };


            return response;
        }


        private static string FormFileName(string productName, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var name = Path.GetFileNameWithoutExtension(fileName);

            var imageNameForStorage = $"{name}--{productName}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return imageNameForStorage;
        }

    }
}