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


        public ReviewsService(IReviewsRepository reviewsRepository)
        {
            _reviewsRepository = reviewsRepository;
            _productsService = new ProductsService(new ProductsRepository(dbContext));
        }

        public async Task<bool> AddReview(Reviews review)
        {
            try
            {
                var status = await _reviewsRepository.AddReview(review);

                if (status)
                {
                    await _productsService.UpdateProductRating(Convert.ToInt32(review.Rating), review.ProductId);
                }

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

        public async Task<int> GetReviewsCount()
        {
            try
            {
                int count = await _reviewsRepository.GetReviewsCount();
                return count;
            }
            catch (Exception ex)
            {
                return 0;
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

        public async Task<int> GetReviewsOfProductCount(int productId)
        {
            try
            {
                int count = await _reviewsRepository.GetReviewsOfProductCount(productId);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> UpdateReview(Reviews review)
        {
            try
            {
                var status = await _reviewsRepository.UpdateReview(review);

                if (status)
                {
                    await _productsService.UpdateProductRating(Convert.ToInt32(review.Rating), review.ProductId);
                }

                return status;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}
