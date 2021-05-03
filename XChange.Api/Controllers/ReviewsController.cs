using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Route("api/review")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IUsersService _usersService;
        private readonly IAuditLogService _auditLogService;
        private readonly IReviewsService _reviewsService;
        private readonly IProductsService _productsService;


        public ReviewsController()
        {
            _usersService = new UsersService(new UsersRepository(dbContext));
            _auditLogService = new AuditLogService(new AuditLogRepository(dbContext));
            _reviewsService = new ReviewsService(new ReviewsRepository(dbContext));
            _productsService = new ProductsService(new ProductsRepository(dbContext));
        }


        /// <summary>
        /// Get Count of all reviews
        /// </summary>
        /// <returns>Count of all reviews</returns>
        /// <response code="200">Count of all reviews</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count", Name = "GetReviewsCount")]
        [AllowAnonymous]
        public async Task<IActionResult> ReviewsCount()
        {
            var count = await _reviewsService.GetReviewsCount();
            return Ok(count);
        }

        /// <summary>
        /// Get Count of all reviews for a given product
        /// </summary>
        /// <returns>Count of all reviews for a given product</returns>
        /// <response code="200">Count of all reviews for a given product</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count/{productId}", Name = "GetProductReviewsCount")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductReviewsCount(int productId)
        {
            var count = await _reviewsService.GetReviewsOfProductCount(productId);
            return Ok(count);
        }

        /// <summary>
        /// Get list of all reviews
        /// </summary>
        /// <returns>Details of all reviews</returns>
        /// <response code="200">Details of all reviews</response>
        /// <response code="400">An error occured, please try again</response>
        [HttpGet(Name = "GetAllReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Reviews()
        {
            var result = await _reviewsService.GetAllReviews();
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(500, "An error ocurred, please try again");
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get Review of a user
        /// </summary>
        /// <returns>Details of all reviews by a given customer</returns>
        /// <response code="200">Details of all reviews by a customer or  User has not written any review yet</response>
        /// <response code="400">An error occurred , please try again</response>
        [HttpGet("users/{userId}", Name = "GetReviewsByUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsByBuyer(int userId)
        {

            var result = await _reviewsService.GetReviewsByUser(userId);
            ApiResponse response;

            if (result != null)
            {
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "User has not written any review yet");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(400, "an error occurred , try again");
                return BadRequest(response);
            }

        }


        /// <summary>
        /// Get all reviews for a product
        /// </summary>
        /// <returns>Details of all reviews for a given product</returns>
        /// <response code="200">Details of all reviews for a given product or  no reviews for product yet</response>
        /// <response code="400">An error occurred , please try again</response>
        [HttpGet("products/{productId}", Name = "GetReviewsForProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsForProduct(int productId)
        {

            var result = await _reviewsService.GetReviewsOfProduct(productId);
            ApiResponse response;

            if (result != null)
            {
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    response = new ApiResponse(200, "no written review for product yet");
                    return Ok(response);
                }
            }
            else
            {
                response = new ApiResponse(400, "an error occurred , try again");
                return BadRequest(response);
            }

        }

        /// <summary>
        /// Get a review
        /// </summary>
        /// <returns>Details of review</returns>
        /// <response code="200">Details of review</response>
        /// <response code="404">Review not found</response>
        [HttpGet("{reviewId}", Name = "GetReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReview(int reviewId)
        {

            var result = await _reviewsService.GetReview(reviewId);
            ApiResponse response;

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                response = new ApiResponse(404, "Review not found");
                return NotFound(response);
            }

        }

        /// <summary>
        /// Adds a new product review
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/review/{productId}
        ///     
        ///     {
        ///        "CustomerReview":"",
        ///        "Rating": "",
        ///     }
        ///
        /// </remarks>
        /// <returns>Added product review success message</returns>
        /// <response code="201">Return new created review</response>
        /// <response code="400">Product not found , Pass in required information , please try again</response>
        [HttpPost("{productId}", Name = "AddReview")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> Reviews(int productId, Review review)
        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            ApiResponse response;
            bool dataValid = true;

            //Get user Id from token
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            //product check
            var checkProduct = await _productsService.GetProduct(productId);

            if (checkProduct == null)
            {
                response = new ApiResponse(400, "Product was not found");
                return BadRequest(response);
            }

            //validate CustomerReview
            if (Validation.IsNull(review.CustomerReview))
            {
                Error err = new Error
                {
                    modelName = "CustomerReview",
                    modelErrorMessgae = "Customer Review is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //validate Rating
            if (Validation.IsNull(review.Rating))
            {
                Error err = new Error
                {
                    modelName = "Rating",
                    modelErrorMessgae = "Rating is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate Rating Length
            if (Validation.IsNull(review.Rating) || review.Rating.Length > 1 || Convert.ToInt32(review.Rating) > 5 || Convert.ToInt32(review.Rating) == 0)
            {
                Error err = new Error
                {
                    modelName = "Rating",
                    modelErrorMessgae = "Rating range should be within 1 - 5",
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

            //create reviews model
            Reviews newReview = new Reviews
            {
                CustomerReview = review.CustomerReview,
                Rating = review.Rating,
                TimeAdded = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                ProductId = productId,
                UserId = Convert.ToInt32(userId)
            };

            var result = await _reviewsService.AddReview(newReview);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), "Added review to product: " + newReview.ProductId);
                _auditLogService.AddAuditLog(auditLog);

                return CreatedAtRoute("GetReview", new { reviewId = newReview.ReviewId }, newReview);

            }
            else
            {
                response = new ApiResponse(400, "adding review failed, please try again");
                return BadRequest(response);
            }

        }


        /// <summary>
        /// Updates a product review
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/review/{productId}
        ///     
        ///     {
        ///        "CustomerReview":"",
        ///        "Rating": "",
        ///     }
        ///
        /// </remarks>
        /// <returns>Updated product review success message</returns>
        /// <response code="201">Return product update success message</response>
        /// <response code="400">Product not found , Pass in required information , please try again</response>
        [HttpPut("{reviewId}", Name = "UpdateReview")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> UpdateReview(int reviewId, Review review)
        {
            ModelError errors;
            List<Error> errorList = new List<Error> { };
            ApiResponse response;
            bool dataValid = true;

            //Get user Id from token
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            //Get review
            var checkReview = await _reviewsService.GetReview(reviewId);

            if (checkReview == null)
            {
                response = new ApiResponse(400, "Review was not found");
                return BadRequest(response);
            }

            if(checkReview.UserId != Convert.ToInt32(userId))
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            //product check
            var checkProduct = await _productsService.GetProduct(checkReview.ProductId);

            if (checkProduct == null)
            {
                response = new ApiResponse(400, "Product was not found");
                return BadRequest(response);
            }

            //validate CustomerReview
            if (Validation.IsNull(review.CustomerReview))
            {
                Error err = new Error
                {
                    modelName = "CustomerReview",
                    modelErrorMessgae = "Customer Review is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //validate Rating
            if (Validation.IsNull(review.Rating))
            {
                Error err = new Error
                {
                    modelName = "Rating",
                    modelErrorMessgae = "Rating is Required",
                };

                errorList.Add(err);
                dataValid = false;
            }

            //Validate Rating Length
            if (Validation.IsNull(review.Rating) || review.Rating.Length > 1 || Convert.ToInt32(review.Rating) > 5 || Convert.ToInt32(review.Rating) == 0)
            {
                Error err = new Error
                {
                    modelName = "Rating",
                    modelErrorMessgae = "Rating range should be within 1 - 5",
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

            //create reviews model
            checkReview.CustomerReview = review.CustomerReview;
            checkReview.Rating = review.Rating;
            checkReview.LastUpdateTime = DateTime.Now;

            var result = await _reviewsService.UpdateReview(checkReview);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), "Updated review for product: " + checkProduct.ProductId);
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Product Review update successful");
                return Ok(response);
            }
            else
            {
                response = new ApiResponse(400, "adding review failed, please try again");
                return BadRequest(response);
            }

        }

        /// <summary>
        /// Deletes a review
        /// </summary>
        /// <returns>Delete success message</returns>
        /// <response code="200">Product review has been deleted successfully</response>
        /// <response code="400">Product review does not exist or you are not eligible to carry out this action</response>
        [HttpDelete("{reviewId}", Name = "DeleteReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [Authorize(Roles = "B")]
        public async Task<IActionResult> Reviews(int reviewId)
        {
            ApiResponse response;

            //Get user Id from token
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.Name).FirstOrDefault().Value;

            //Get review
            var checkReview = await _reviewsService.GetReview(reviewId);

            if (checkReview == null)
            {
                response = new ApiResponse(400, "Product Review was not found");
                return BadRequest(response);
            }

            if (checkReview.UserId != Convert.ToInt32(userId))
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

            //product check
            var checkProduct = await _productsService.GetProduct(checkReview.ProductId);

            if (checkProduct == null)
            {
                response = new ApiResponse(400, "Product was not found");
                return BadRequest(response);
            }

            var result = await _reviewsService.DeleteReview(Convert.ToInt32(userId), reviewId);

            if (result)
            {
                //add audit log
                AuditLog auditLog = Utility.Utility.AddAuditLog(Convert.ToInt32(userId), "Deleted product review: " + reviewId);
                _auditLogService.AddAuditLog(auditLog);

                response = new ApiResponse(200, "Product review has been deleted successfully");
                return Ok(response);
            }
            else
            {
                response = new ApiResponse(400, "You are not eligible to carry out this action");
                return BadRequest(response);
            }

        }
    }
}