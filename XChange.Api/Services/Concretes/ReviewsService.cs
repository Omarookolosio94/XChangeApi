using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XChange.Api.Models;
using XChange.Api.Repositories.Concretes;
using XChange.Api.Repositories.Interfaces;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class ReviewsService : IReviewsService
    {
        private readonly XChangeDatabaseContext dbContext = new XChangeDatabaseContext();
        private readonly IReviewsRepository _reviewsRepository;
        private readonly IProductsService _productsService;


        public ReviewsService(IReviewsRepository reviewsRepository , IProductsRepository productsRepository)
        {
            _reviewsRepository = reviewsRepository;
            _productsService = new ProductsService(new ProductsRepository(dbContext));
        }

        public async Task<bool> AddReview(Reviews review)
        {
            try
            {
                var status = await _reviewsRepository.AddReview(review);
                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> DeleteReview(int userId, int reviewId)
        {
            try
            {
                var status = await _reviewsRepository.DeleteReview(userId, reviewId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Reviews>> GetAllReviews()
        {
            try
            {
                var status = await _reviewsRepository.GetAllReviews();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Reviews> GetReview(int reviewId)
        {
            try
            {
                var status = await _reviewsRepository.GetReview(reviewId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Reviews>> GetReviewsByUser(int userId)
        {
            try
            {
                var status = await _reviewsRepository.GetReviewsByUser(userId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Reviews>> GetReviewsOfProduct(int productId)
        {
            try
            {
                var status = await _reviewsRepository.GetReviewsOfProduct(productId);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateReview(int userId , string productPreviousRating,  Reviews review)
        {
            try
            {
                bool result = false;
                Reviews updateReview = await _reviewsRepository.GetSingleReviewByUser(userId, review.ReviewId);

                if (updateReview != null)
                {
                    if (review.Rating.Length > 0)
                    {

                        var newProductRating = Utility.Utility.CalculateUpdateRating(productPreviousRating , updateReview.Rating , review.Rating);
                        await _productsService.UpdateProductRatingReview(newProductRating, review.ProductId);

                        updateReview.Rating = review.Rating;
                    }

                    updateReview.CustomerReview = review.CustomerReview;
                    updateReview.LastUpdateTime = DateTime.Now;


                    result = await _reviewsRepository.UpdateReview(updateReview);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
